using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
using System.Reflection;

namespace WebApplication1.Swagger
{
    public class SwaggerHelper
    {
        public static void ConfigureService(IServiceCollection service)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio
            // Register the Swagger generator, defining 1 or more Swagger documents
            service.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    Description = "up genic Api",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Core Identity",
                    Version = "v1",
                    Description = "Using ASP.NET Core Identity Web API With JWT, TFA Authenticator and Swagger",
                    Contact = new OpenApiContact
                    {
                        Name = "Sander Hammelburg",
                        Url = new Uri("https://github.com/shammelburg/CoreIdentity")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
    



