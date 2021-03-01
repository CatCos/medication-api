namespace MedicationApi.Repositories.Read
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;
    using MongoDB.Driver;

    public class MedicationReadRepository : IMedicationReadRepository
    {
        private readonly IMongoCollection<Medication> collection;

        public MedicationReadRepository(IMongoDatabase database)
        {
            this.collection = database.GetCollection<Medication>("Medications");
        }

        public async Task<Medication> GetMedicationByIdAsync(Guid medicationId)
        {
            var builder = Builders<Medication>.Filter;
            FilterDefinition<Medication> filterDefinition = builder.Eq(b => b.Id, medicationId);

            return await this.collection.FindSync(filterDefinition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync(
            int offset,
            int limit)
        {
            var builder = Builders<Medication>.Filter;
            FilterDefinition<Medication> filterDefinition = Builders<Medication>.Filter.Empty;

            return await collection.Find(filterDefinition)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();
        }
    }
}
