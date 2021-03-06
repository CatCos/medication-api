﻿namespace MedicationApi.Repositories.Write
{
    using System;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;
    using MongoDB.Driver;

    public class MedicationWriteRepository : BaseRepository, IMedicationWriteRepository
    {
        public MedicationWriteRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task InsertAsync(Medication medication)
        {
            await this.collection.InsertOneAsync(medication);
        }

        public async Task DeleteAsync(Guid medicationId)
        {
            var builder = Builders<Medication>.Filter;
            var filterDefinition = builder.Eq(b => b.Id, medicationId);

            await this.collection.DeleteManyAsync(filterDefinition);
        }
    }
}
