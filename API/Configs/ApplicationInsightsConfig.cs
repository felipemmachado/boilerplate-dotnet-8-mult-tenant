using System.Diagnostics.CodeAnalysis;
namespace API.Configs
{
    public static class ApplicationInsightsConfig
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration["ApplicationInsights:connectionString"]))
            {
                services.AddApplicationInsightsTelemetry(options =>
                {
                    options.ConnectionString = configuration["ApplicationInsights:connectionString"];
                });

            }

            return services;
        }
    }
}
