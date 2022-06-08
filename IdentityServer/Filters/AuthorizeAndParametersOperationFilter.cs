using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Identity.Filters
{
    public class AuthorizeAndParametersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //custrom header filter
            //https://stackoverflow.com/questions/41180615/how-to-send-custom-headers-with-requests-in-swagger-ui
            //if (operation.Parameters == null)
            //    operation.Parameters = new List<OpenApiParameter>();

            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "Issuer",
            //    In = ParameterLocation.Header,
            //    Required = false,
            //    Description = "Alliance Issuer dev Key : fTyvGMnk1EaeZhiH+JZmMw==",

            //});

            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1425
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);

            if (methodInfo == null)
            {
                return;
            }

            var hasAuthorizeAttribute = false;

            if (methodInfo.MemberType == MemberTypes.Method)
            {
                // NOTE: Check the controller itself has Authorize attribute
                hasAuthorizeAttribute = methodInfo.DeclaringType == null ? false
                    : methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                // NOTE: Controller has Authorize attribute, so check the endpoint itself.
                //       Take into account the allow anonymous attribute
                if (hasAuthorizeAttribute)
                {
                    hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                }
                else
                {
                    hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
                }
            }

            if (!hasAuthorizeAttribute)
            {
                return;
            }

            // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
            //       we can also pass through the names of the policies in the string[]
            //       which will indicate which permission you require.
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}
