using BeetrootCqrs.DAL.DB;
using BeetrootCqrs.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BeetrootCqrs.DAL
{
    public static class DependencyInjection
    {
        public static void AddDbContextExt(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString, b => b.MigrationsAssembly("BeetrootCqrs.DAL"))
                );

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetService<AppDbContext>());
        }
    }
}
