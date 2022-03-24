namespace WebHost.Middlewares
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public static partial class SwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // c.SwaggerDoc("v1", new OpenApiInfo { Title = "Developer API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Developer API",
                    Description = "API Public for develop research and shareing",
                    TermsOfService = new Uri("https://tuanitpro.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Tuan Le Thanh",
                        Email = string.Empty,
                        Url = new Uri("https://tuanitpro.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/mit-license.php"),
                    },
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Developer API",
                    Description = "API Public for develop research and shareing",
                    TermsOfService = new Uri("https://tuanitpro.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Tuan Le Thanh",
                        Email = string.Empty,
                        Url = new Uri("https://tuanitpro.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/mit-license.php"),
                    },
                });
            });

            return services;
        }
    }
}