namespace MedicationApi.Repositories.Read
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;
    using MongoDB.Driver;

    public class MedicationReadRepository : BaseRepository, IMedicationReadRepository
    {
        public MedicationReadRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<Medication?> GetMedicationByIdAsync(Guid medicationId)
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
            FilterDefinition<Medication> filterDefinition = builder.Empty;

            return await collection.Find(filterDefinition)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();
        }
    }
}
