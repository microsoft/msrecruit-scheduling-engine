//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.Product.ServicePlatform.AspNetCore.Builder
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Common.Product.ServicePlatform.AspNetCore.Builder.Filters;
    using Common.Product.ServicePlatform.Utils;
    using ServicePlatform.AspNetCore.Builder.Filters;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class UseSwaggerExtensions
    {        
        public static IApplicationBuilder UseSwaggerInGTA(this IApplicationBuilder builder, string appName)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} API");
            });
            return builder;
        }
        public static IApplicationBuilder UseSwaggerInGTA(this IApplicationBuilder builder, ApplicationPublishDetails applicationPublishDetails)
        {
            builder.UseSwagger();
            if (applicationPublishDetails != null)
            {
                builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(applicationPublishDetails.DocumentationRoute, applicationPublishDetails.ApplicationName);
                });
            }
            return builder;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string appName)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilterDescriptors = new List<FilterDescriptor>();
                c.AddSecurityDefinition(
                    "Bearer",
                    new ApiKeyScheme()
                    {
                        In = "header",
                        Description = "Please insert JWT with Bearer into field, You can get a JWT token from any other GTA app like (Offer, Offer Rule, Refer) by looking at REST requests made by the browser to the application backend",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
                c.SwaggerDoc("v1", new Info { Title = $"{appName} Service API", Version = "v1" });
                c.OperationFilter<HeaderOperationFilter>();
                c.OperationFilter<SwaggerFileUploadOperation>();
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, ApplicationPublishDetails applicationPublishDetails)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilterDescriptors = new List<FilterDescriptor>();
                c.OperationFilter<HeaderOperationFilter>();
                c.OperationFilter<SwaggerFileUploadOperation>();
                if (applicationPublishDetails != null)
                {
                    if (applicationPublishDetails.IsAuthorizedByBearer)
                    {
                        c.AddSecurityDefinition(
                            "Bearer",
                            new ApiKeyScheme()
                            {
                                In = "header",
                                Description = "Please insert JWT with Bearer into field, You can get a JWT token from any other GTA app by looking at REST requests made by the browser to the application backend",
                                Name = "Authorization",
                                Type = "apiKey"
                            });
                        c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> 
                        {
                            { "Bearer", Enumerable.Empty<string>() },                            
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(applicationPublishDetails.Version))
                    {
                        c.SwaggerDoc(applicationPublishDetails.Version, new Info
                        {
                            Title = applicationPublishDetails.ApplicationName,
                            Version = applicationPublishDetails.Version,
                            Description = applicationPublishDetails.ApplicationDescription,
                            Contact = new Contact() 
                            { 
                                Email = applicationPublishDetails.Contact, 
                                Name = applicationPublishDetails.ContactName,
                                Url = applicationPublishDetails.ContactUrl,
                            },
                            License = new License()
                            {
                                Name = "Microsoft Corporation",
                            },
                            TermsOfService = "https://www.microsoft.com/en-us/servicesagreement/",                            
                        });
                    }
                }
            });
            return services;
        }
    }
}
