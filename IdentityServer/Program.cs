using Identity.Configuration;
using Identity.Entities;
using Identity.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// build AppSettings
var appSettings = new ConfigurationService(AppContext.BaseDirectory).GetAppSettings();

// CORS policy
if (appSettings.CorsOrigins != null && appSettings.CorsOrigins.Any())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.WithOrigins(appSettings.CorsOrigins.ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    });
}

// dbcontext
builder.Services.AddDbContext<IdentityContext>(opts => opts
    .UseNpgsql(ConnectionStringMapper.GetConnectionString(appSettings))
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging());

// 응답 압축
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes =
        ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/json", "text/plain", "image/svg+xml" });
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

// swagger 설정
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = appSettings.Swagger?.Title,
        Version = appSettings.Swagger?.Version,
        Description = appSettings.Swagger?.Description,
        Contact = new OpenApiContact
        {
            Email = string.Empty,
            Url = new Uri(appSettings.Swagger?.Link ?? String.Empty),
        }
    });
    c.EnableAnnotations();
    //c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    //{
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "bearer",
    //});
    c.OperationFilter<AuthorizeAndParametersOperationFilter>();
    // swagger에 표시해줄 정보를 생성하기 위해 빌드시 xml파일 생성
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// DI Settings
builder.Services.AddSingleton(appSettings);
builder.Services.AddScoped<IIdentityContext, IdentityContext>();


var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer");
        //c.RoutePrefix = string.Empty; //if swagger show webroot
    });
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
//https://www.c-sharpcorner.com/article/how-to-implement-jwt-authentication-in-web-api-using-net-6-0-asp-net-core/
