namespace MedicationApi.Application.Handlers.Commands
{
    using System;
    using System.Threading.Tasks;
    using MedicationApi.Application.Exceptions;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Repositories.Read;
    using MedicationApi.Repositories.Write;
    using Microsoft.Extensions.Logging;

    public class DeleteMedicationCommandHandler : ICommandHandler<DeleteMedicationCommand>
    {
        private readonly IMedicationWriteRepository medicationWriteRepository;

        private readonly IMedicationReadRepository medicationReadRepository;

        private readonly ILogger logger;

        public DeleteMedicationCommandHandler(
            IMedicationWriteRepository medicationWriteRepository,
            IMedicationReadRepository medicationReadRepository,
            ILogger<DeleteMedicationCommandHandler> logger)
        {
            this.medicationWriteRepository = medicationWriteRepository;
            this.medicationReadRepository = medicationReadRepository;
            this.logger = logger;
        }

        public async Task HandleAsync(DeleteMedicationCommand command)
        {
            try
            {
                var medication = await this.medicationReadRepository.GetMedicationByIdAsync(command.Id);

                if (medication == null)
                {
                    throw new NotFoundException();
                }

                await this.medicationWriteRepository.DeleteAsync(command.Id);
            }
            catch (Exception)
            {
                this.logger.LogError("Failed to delete medication.");

                throw;
            }
        }
    }
}
