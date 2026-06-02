using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CRM.Web.ExtensionClasses
{
    public static class PresentationExtensions
    {
        public static IServiceCollection AddPresentationAndSwagger(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "Enter 'Bearer' followed by a space and your token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Id   = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] {}
                }
            });
            });

            return services;
        }

        public static WebApplication UseSwaggerWithUI(this WebApplication app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("v1/swagger.json", "CRM API v1");
                options.DocumentTitle = "CRM API";
                options.ConfigObject = new ConfigObject
                {
                    DisplayRequestDuration = true
                };
                options.DocExpansion(DocExpansion.List);
                options.EnableFilter();
                options.DefaultModelExpandDepth(-1);
                options.EnableDeepLinking();
                options.EnableTryItOutByDefault();
                options.EnablePersistAuthorization();
            });

            // Redirect root "/" to Swagger UI automatically
            app.MapGet("/", (HttpContext ctx) =>
               Results.Redirect($"{ctx.Request.PathBase}/swagger"))
                      .ExcludeFromDescription();

            return app;
        }
    }
}
