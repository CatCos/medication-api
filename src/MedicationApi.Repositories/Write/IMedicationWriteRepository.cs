namespace MedicationApi.Repositories.Write
{
    using System;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;

    public interface IMedicationWriteRepository
    {
        Task InsertAsync(Medication medication);

        Task DeleteAsync(Guid medicationId);
    }
}
