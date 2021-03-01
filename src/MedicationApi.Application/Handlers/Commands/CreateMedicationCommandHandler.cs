namespace MedicationApi.Application.Handlers.Commands
{
    using System;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Repositories.Write;
    using Microsoft.Extensions.Logging;

    public class CreateMedicationCommandHandler : ICommandHandler<CreateMedicationCommand>
    {
        private readonly IMedicationWriteRepository medicationWriteRepository;

        private readonly ILogger logger;

        public CreateMedicationCommandHandler(
            IMedicationWriteRepository medicationWriteRepository,
            ILogger<CreateMedicationCommandHandler> logger)
        {
            this.medicationWriteRepository = medicationWriteRepository;
            this.logger = logger;
        }

        public async Task HandleAsync(CreateMedicationCommand command)
        {
            try
            {
                await this.medicationWriteRepository.InsertAsync(command.Medication);
            }
            catch (Exception)
            {
                this.logger.LogError("Failed to create medication.");

                throw;
            }
        }
    }
}
