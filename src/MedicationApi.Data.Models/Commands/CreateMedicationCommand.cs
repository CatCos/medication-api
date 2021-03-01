namespace MedicationApi.Data.Models.Commands
{
    using MedicationApi.Data.Models.Entities;

    public class CreateMedicationCommand
    {
        public Medication Medication { get; set; }
    }
}
