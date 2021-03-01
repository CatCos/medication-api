namespace MedicationApi.Application.Validations
{
    using System;
    using FluentValidation;
    using MedicationApi.Contracts;

    public sealed class MedicationValidator : AbstractValidator<MedicationDto>
    {
        public MedicationValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(m => m.Id)
                .NotEmpty()
                .Must(x => x != Guid.Empty)
                .WithMessage("'Id' must not be empty.");

            this.RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("'Name' must not be empty.");

            this.RuleFor(m => m.Quantity)
                .GreaterThan(0)
                .WithMessage("'Quantity' must be greater than 0.");
        }
    }
}
