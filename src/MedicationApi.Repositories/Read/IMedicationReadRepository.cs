namespace MedicationApi.Repositories.Read
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;

    public interface IMedicationReadRepository
    {
        Task<IEnumerable<Medication>> GetAllMedicationsAsync(
            int offset,
            int limit);

        Task<Medication> GetMedicationByIdAsync(Guid medicationId);
    }
}
