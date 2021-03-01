namespace MedicationApi.Application.Handlers.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using MedicationApi.Repositories.Read;
    using Microsoft.Extensions.Logging;

    public class GetMedicationsQueryHandler : IQueryHandler<GetMedicationsQuery, IEnumerable<Medication>>
    {
        private readonly IMedicationReadRepository medicationReadRepository;

        private readonly ILogger logger;

        public GetMedicationsQueryHandler(
            IMedicationReadRepository medicationReadRepository,
            ILogger<GetMedicationsQueryHandler> logger)
        {
            this.medicationReadRepository = medicationReadRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<Medication>> HandleAsync(GetMedicationsQuery query)
        {
            try
            {
                return await this.medicationReadRepository.GetAllMedicationsAsync(
                    query.Offset,
                    query.Limit);
            }
            catch (Exception)
            {
                this.logger.LogError("Failed to get all medications.");

                throw;
            }
        }
    }
}
