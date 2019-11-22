using Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Interfaces;
using Repositories;
using Repositories.Interfaces;
using Network.Interfaces;
using Network;
using Middlewares;

namespace Service.Geolocation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IGeolocationRepository, InMemoryGeolocationRepository>();
            services.AddScoped<IGeolocationService, GeolocationService>();
            services.AddScoped<INetworkAddress, NetworkAddress>();
            services.AddScoped<IIpDetailsService, IpDetailsService>();
            services.Configure<IpDetailsServiceSettings>(Configuration.GetSection("ipstack"));
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Geolocation API";
                    document.Info.Description = "Geolocation demo";
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
