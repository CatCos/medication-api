namespace MedicationApi.Configuration
{
    using MedicationApi.Configuration.Interfaces;

    public class ApplicationSettings : IApplicationSettings
    {
        public MongoSettings MongoSettings { get; set; }
    }
}
