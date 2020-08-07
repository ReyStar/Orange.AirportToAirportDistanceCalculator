using DistanceCalculator.API;
using DistanceCalculator.Application;
using DistanceCalculator.Domain;
using DistanceCalculator.Repository;
using DistanceCalculator.Shell.Configuration;
using DistanceCalculator.Shell.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace DistanceCalculator.Shell
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterDomain(Configuration);
            services.RegisterApplication(Configuration);
            services.RegisterRepository(Configuration);
            services.RegisterApi(Configuration)
                    .ConfigureJsonFormat();

            services.RegisterShellDependencies(Configuration);
            
            services.RegisterAutoMapper();

            services.ConfigureApiVersioning();
            services.AddAllOpenApiDocumentWithVersions();
            services.RegisterTracing(Configuration);
            services.RegisterMetrics(Configuration);
            services.AddHeaderPropagation();

            services.Configure<MemoryCacheOptions>(Configuration.GetSection(nameof(MemoryCacheOptions)));
            services.AddMemoryCache();

            services.AddHealthChecks().AddCheck<HealthCheck>("base_health_check");

            services.AddSingleton<IStartupFilter, StartupValidation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHeaderPropagation();
            app.UseMetrics();
            
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                AllowCachingResponses = false
            });
  
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor 
                                   | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();

                app.UseDeveloperExceptionPage();
            }
           
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
