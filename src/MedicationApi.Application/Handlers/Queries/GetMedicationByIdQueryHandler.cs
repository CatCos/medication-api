namespace MedicationApi.Application.Handlers.Queries
{
    using System;
    using System.Threading.Tasks;
    using MedicationApi.Application.Exceptions;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using MedicationApi.Repositories.Read;
    using Microsoft.Extensions.Logging;

    public class GetMedicationByIdQueryHandler : IQueryHandler<GetMedicationByIdQuery, Medication>
    {
        private readonly IMedicationReadRepository medicationReadRepository;

        private readonly ILogger logger;

        public GetMedicationByIdQueryHandler(
            IMedicationReadRepository medicationReadRepository,
            ILogger<GetMedicationByIdQueryHandler> logger)
        {
            this.medicationReadRepository = medicationReadRepository;
            this.logger = logger;
        }

        public async Task<Medication> HandleAsync(GetMedicationByIdQuery query)
        {
            try
            {
                var result = await this.medicationReadRepository.GetMedicationByIdAsync(query.Id);

                if (result == null)
                {
                    throw new NotFoundException();
                }

                return result;
            }
            catch (Exception)
            {
                this.logger.LogError("Failed to get medication by id.");

                throw;
            }
        }
    }
}
