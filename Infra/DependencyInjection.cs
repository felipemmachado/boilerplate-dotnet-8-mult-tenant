using Application.Common.Interfaces;
using Infra.Persistence;
using Infra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("AppConnection"),
                    p =>
                    {
                        p.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        p.EnableRetryOnFailure(maxRetryCount: 4);
                    });
            });


            return services;
        }
    }
}
