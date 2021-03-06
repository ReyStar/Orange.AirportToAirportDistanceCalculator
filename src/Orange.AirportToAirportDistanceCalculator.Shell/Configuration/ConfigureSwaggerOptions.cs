﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Orange.AirportToAirportDistanceCalculator.Shell.Configuration
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    static class SwaggerConfig
    {
        public static IServiceCollection AddAllOpenApiDocumentWithVersions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOpenApiDocument();// Register default providers

            //Register all documents
            serviceCollection.AddSingleton<IEnumerable<OpenApiDocumentRegistration>>(provider =>
            {
                var versionProvider =
                    (IApiVersionDescriptionProvider) provider.GetService(typeof(IApiVersionDescriptionProvider));
                
                var registrations = new List<OpenApiDocumentRegistration>();
                
                foreach (var apiVersionDescription in versionProvider.ApiVersionDescriptions)
                {
                    var versionValue = $"v{apiVersionDescription.ApiVersion.MajorVersion}";

                    registrations.Add(new OpenApiDocumentRegistration(versionValue,
                        new AspNetCoreOpenApiDocumentGenerator(GetGenerationSettings(versionValue))));
                };

                return registrations;
            });

            return serviceCollection;
        }

        private static AspNetCoreOpenApiDocumentGeneratorSettings GetGenerationSettings(string versionValue)
        {
            var documentSettings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                DocumentName = versionValue,
                ApiGroupNames = new[] {versionValue},
                PostProcess = UpdateInfoForApiVersion
            };

            // Add an authenticate button to Swagger for JWT tokens
            documentSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
            documentSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT", new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            }));

            return documentSettings;
        }

        static void UpdateInfoForApiVersion(OpenApiDocument document)
        {
            document.Info = new OpenApiInfo
            {
                Title = "Airport To airport distance calculator",
                Description = "This service calculate and storage distances between airports use IATA codes",
                Contact = new OpenApiContact
                {
                    Name = "Andrey Star",
                    Email = "starandrey@hotmail.com"
                },
                TermsOfService = "https://github.com/ReyStar/Orange.AirportToAirportDistanceCalculator",
                License = new OpenApiLicense { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };
        }
    }
}
