using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BeetrootCqrs.API.Extentions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BeetrootCqrs.DAL;
using BeetrootCqrs.BLL.Interfaces;
using BeetrootCqrs.BLL.Services;
using BeetrootCqrs.API.Services;
using System;
using MediatR;
using BeetrootCqrs.BLL.Configurations;

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
            services.Configure<UdpConfiguration>(_configuration.GetSection("UdpConfiguration"));
            services.AddDbContextExt(_configuration.GetConnectionString("DefaultConnection"));

            services.AddSingleton<IUdpReceiveService, UdpReceiveService>();
            services.AddHostedService<UdpHostedService>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("BeetrootCqrs.DAL"));
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
