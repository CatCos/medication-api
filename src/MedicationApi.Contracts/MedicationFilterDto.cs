namespace MedicationApi.Contracts
{
    public class MedicationFilterDto
    {
        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 100;
    }
}
