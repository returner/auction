using Identity.Configuration.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Identity.Configuration
{
    public class ConfigurationService
    {
        private const string DEV_ENVIRONMENT_NAME = "ASPNETCORE_ENVIRONMENT";
        
        private readonly string _environmentName;
        private readonly string? _currentDirectory;
        private readonly IConfiguration _configuration;

        public ConfigurationService(string? currentDirectory = null)
        {
            _currentDirectory = currentDirectory ?? Directory.GetCurrentDirectory();

            // 환경변수에 값이 세팅되어 있지 않다면 운영환경
            _environmentName = Environment.GetEnvironmentVariable(DEV_ENVIRONMENT_NAME) ?? string.Empty;

            _configuration = new ConfigurationBuilder()
                .SetBasePath(_currentDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IAppSettings GetAppSettings()
        {
            var appSettings = new AppSettings 
            { 
                CorsOrigins = GetConfigurationValue<IEnumerable<string>>("CorsOrigins"),
                Database = new DatabaseSetting
                {
                    Type = GetConfigurationValue<DatabaseType>("Database:Type"),
                    Hostname = GetConfigurationValue<string>("Database:Hostname"),
                    Port = GetConfigurationValue<int>("Database:Port"),
                    Username = GetConfigurationValue<string>("Database:Username"),
                    Password = GetConfigurationValue<string>("Database:Password"),
                    DbName = GetConfigurationValue<string>("Database:DbName"),
                },
                Swagger = new SwaggerSetting
                {
                    Title = GetConfigurationValue<string>("Swagger:Title"),
                    Version = GetConfigurationValue<string>("Swagger:Version"),
                    Description = GetConfigurationValue<string>("Swagger:Description"),
                    Link = GetConfigurationValue<string>("Swagger:Link"),
                }
            };

            return appSettings;
        }

        public string GetEnvironment()
        {
            return _environmentName;
        }

        private T GetConfigurationValue<T>(string key)
        {
            return _configuration.GetSection($"App:{key}").Get<T>();
        }
    }
}
