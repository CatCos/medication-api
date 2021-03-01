namespace MedicationApi.Contracts
{
    using System;

    public class MedicationDto
    {
        public Guid Id { get; set; } 

        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
