using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGAware.Security.JWT.Extensions;

public static class SwaggerJWTExtension
{
    public static IServiceCollection AddSwaggerGenJwt(
        this IServiceCollection services,
        string versionApi, OpenApiInfo apiInfo)
    {
        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<ODataOperationFilter>();

            c.SwaggerDoc(versionApi, apiInfo);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization Header - Bearer Authentication.\r\n\r\n" +
                    "Use 'Bearer' [espaço] and token.\r\n\r\n" +
                    "Example: 'Bearer YourToken'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}




    public class ODataOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && descriptor.FilterDescriptors.Any(filter => filter.Filter is Microsoft.AspNetCore.OData.Query.EnableQueryAttribute))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$select",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Returns only the selected properties. (ex. FirstName, LastName, City)",
                    Required = false
                });

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$expand",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Include only the selected objects. (ex. Childrens, Locations)",
                    Required = false
                });

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$filter",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Filter the response with OData filter queries.",
                    Required = false
                });

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$top",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Number of objects to return. (ex. 25)",
                    Required = false
                });

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$skip",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Number of objects to skip in the current order (ex. 50)",
                    Required = false
                });

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "$orderby",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                    Description = "Define the order by one or more fields (ex. LastModified)",
                    Required = false
                });

            }
        }
    }