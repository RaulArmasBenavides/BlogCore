using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
         IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConexionSQL");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                  ServiceLifetime.Scoped);

            // Configuración para Oracle
            //services.AddDbContext<OracleDBContext>(options =>
            //    options.UseOracle(
            //        configuration.GetConnectionString("ConexionOracle"),
            //        b => b.MigrationsAssembly(typeof(OracleDBContext).Assembly.FullName)),
            //    ServiceLifetime.Scoped);

            // Configuración para PostgreSQL
            //services.AddDbContext<PostgreSqlContext>(options =>
            //    options.UseNpgsql(
            //        configuration.GetConnectionString("ConexionPostgreSQL"),
            //        b => b.MigrationsAssembly(typeof(PostgreSqlContext).Assembly.FullName)),
            //    ServiceLifetime.Scoped);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //Agregar contenedor de trabajo
            services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();
            return services;
        }
    }
}
