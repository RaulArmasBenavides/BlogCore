using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace BlogCore.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IMovieService, MovieService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<ICategoryService, CategorieService>();

            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IMovieRepository, MovieRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();

            //services.AddAuthorization(options => options.DefaultPolicy =
            //new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
            //var key = configuration.GetValue<string>("ApiSettings:Secreta");
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateIssuerSigningKey = true,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero,
            //    };
            //});



        }
    }
}
