using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BeetrootCqrs.API.Extentions
{
    public static class SwaggerExtensions
    {
        const string AppTitle = "Beetroot CQRS Test Task API";
        public static void AddSwaggerServiceExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = AppTitle, Version = "v1" });
            });
        }

        public static void UseSwaggerMiddlewareExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", AppTitle);
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
