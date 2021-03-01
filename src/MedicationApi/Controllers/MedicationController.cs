namespace MedicationApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MedicationApi.Application.Handlers.Commands;
    using MedicationApi.Application.Handlers.Queries;
    using MedicationApi.Contracts;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("medications")]
    public class MedicationController : ControllerBase
    {
        private readonly IValidator<MedicationDto> validator;

        private readonly IValidator<MedicationFilterDto> filterValidator;

        private readonly IMapper mapper;

        public MedicationController(
            IValidator<MedicationDto> validator,
            IValidator<MedicationFilterDto> filterValidator,
            IMapper mapper)
        {
            this.validator = validator;
            this.filterValidator = filterValidator;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetMedicationsAsync(
            [FromServices] IQueryHandler<GetMedicationsQuery, IEnumerable<Medication>> handler,
            [FromQuery] MedicationFilterDto filter)
        {
            var validationResult = this.filterValidator.Validate(filter);

            if (!validationResult.IsValid)
            {
                return this.BadRequest();
            }

            var query = mapper.Map<GetMedicationsQuery>(filter);

            var medications = await handler.HandleAsync(query);

            var pagedResultDto = this.mapper.Map<IEnumerable<MedicationDto>>(medications);

            return this.Ok(pagedResultDto);
        }

        [HttpGet("{medicationId}", Name = "GetMedicationById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MedicationDto>> GetMedicationByIdAsync(
            [FromServices] IQueryHandler<GetMedicationByIdQuery, Medication> handler,
            Guid medicationId)
        {
            if (medicationId == null || medicationId == Guid.Empty)
            {
                return this.BadRequest();
            }

            var query = new GetMedicationByIdQuery
            {
                Id = medicationId
            };

            var medication = await handler.HandleAsync(query);

            var medicationDto = this.mapper.Map<MedicationDto>(medication);

            return this.Ok(medicationDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMedicationAsync(
            [FromServices] ICommandHandler<CreateMedicationCommand> handler,
            [FromBody] MedicationDto medication)
        {
            var validationResult = this.validator.Validate(medication);

            if (!validationResult.IsValid)
            {
                return this.BadRequest();
            }

            var command = this.mapper.Map<CreateMedicationCommand>(medication);

            await handler.HandleAsync(command);

            return this.CreatedAtAction("GetMedicationById", new { medicationId = medication.Id }, null);
        }

        [HttpDelete("{medicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMedicationAsync(
            [FromServices] ICommandHandler<DeleteMedicationCommand> handler,
            Guid medicationId)
        {
            if (medicationId == null || medicationId == Guid.Empty)
            {
                return this.BadRequest();
            }

            var command = new DeleteMedicationCommand
            {
                Id = medicationId
            };

            await handler.HandleAsync(command);

            return this.NoContent();
        }
    }
}
