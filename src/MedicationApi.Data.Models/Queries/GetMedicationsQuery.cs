namespace MedicationApi.Data.Models.Queries
{
    public class GetMedicationsQuery
    {
        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 100;
    }
}
