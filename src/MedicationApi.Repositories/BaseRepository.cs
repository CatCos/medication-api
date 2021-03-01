namespace MedicationApi.Repositories
{
    using MedicationApi.Data.Models.Entities;
    using MongoDB.Driver;

    public class BaseRepository
    {
        public readonly IMongoCollection<Medication> collection;

        public BaseRepository(IMongoDatabase database)
        {
            this.collection = database.GetCollection<Medication>("Medications");
        }
    }
}
