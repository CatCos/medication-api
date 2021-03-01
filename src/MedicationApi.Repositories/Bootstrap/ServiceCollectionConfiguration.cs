namespace MedicationApi.Repositories.Bootstrap
{
    using MedicationApi.Configuration;
    using MedicationApi.Repositories.Read;
    using MedicationApi.Repositories.Write;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public static class ServiceCollectionConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, MongoSettings mongoDbSettings)
        {
            services.AddSingleton<IMedicationReadRepository, MedicationReadRepository>();
            services.AddSingleton<IMedicationWriteRepository, MedicationWriteRepository>();

            services.AddMongoDb(mongoDbSettings);

            return services;
        }

        private static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            MongoSettings mongoDbSettings)
        {
            services.AddSingleton<IMongoClient>(_ =>
            {
                var mongoClientSettings = new MongoClientSettings
                {
                    Servers = new[] { MongoServerAddress.Parse(mongoDbSettings.ConnectionString) }
                };

                return new MongoClient(mongoClientSettings);
            });

            services.AddSingleton(p => p.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.Name));

            return services;
        }
    }
}
