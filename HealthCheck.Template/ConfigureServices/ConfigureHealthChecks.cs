namespace Asp.Net.Core.HealthCheck.ConfigureServices
{
    public static class ConfigureHealthChecks
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    name: "SQL Database Check",
                    tags: new string[] { "JAMS Database" });

            return services;
        }
    }
}
