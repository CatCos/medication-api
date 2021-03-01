namespace MedicationApi.Application.Validations
{
    using FluentValidation;
    using MedicationApi.Contracts;

    public class MedicatonFilterValidator : AbstractValidator<MedicationFilterDto>
    {
        public MedicatonFilterValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(f => f.Offset)
                .GreaterThanOrEqualTo(0);

            this.RuleFor(f => f.Limit)
               .GreaterThan(0);
        }
    }
}
