using Application.Common.Configs;
namespace API.Configs
{
    public static class LoadConfigs
    {
        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.Configure<JwtPasswordConfig>(configuration.GetSection("JwtPasswordConfig"));

            services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));

            services.Configure<FileConfig>(configuration.GetSection("FileConfig"));

            services.Configure<ApiConfig>(configuration.GetSection("ApiConfig"));

            return services;
        }
    }
}
