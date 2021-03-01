namespace MedicationApi.Application.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Medication not found.")
        {
        }
    }
}
