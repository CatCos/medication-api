namespace MedicationApi.Configuration.Interfaces
{
    public interface IApplicationSettings
    {
        public MongoSettings MongoSettings { get; set; }
    }
}
