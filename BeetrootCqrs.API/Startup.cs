using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BeetrootCqrs.API.Extentions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BeetrootCqrs.DAL;

namespace BeetrootCqrs.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerServiceExt();
            services.AddCors();
            services.AddDbContextExt(_configuration.GetConnectionString("DefaultConnection"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandlerExt(logger);
            app.UseCorsMiddlewareExt();
            app.UseRouting();
            app.UseSwaggerMiddlewareExt();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
