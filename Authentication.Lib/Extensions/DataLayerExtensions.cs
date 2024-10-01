using Authentication.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Lib.Extensions
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseMySql(config.GetConnectionString("AuthenticationDatabaseConnection"),
                    new MySqlServerVersion(new Version(10, 6, 9)),
                    mysqlOptions =>
                    {
                        mysqlOptions.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName);
                    }));
            return services;
        }
    }
}
