namespace MedicationApi.Application.Bootstrap
{
    using System.Collections.Generic;
    using FluentValidation;
    using MedicationApi.Application.Handlers.Commands;
    using MedicationApi.Application.Handlers.Queries;
    using MedicationApi.Application.Middlewares;
    using MedicationApi.Application.Validations;
    using MedicationApi.Contracts;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<CreateMedicationCommand>, CreateMedicationCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteMedicationCommand>, DeleteMedicationCommandHandler>();
            services.AddScoped<
                IQueryHandler<GetMedicationsQuery, IEnumerable<Medication>>, GetMedicationsQueryHandler>();
            services.AddScoped<IQueryHandler<GetMedicationByIdQuery, Medication>, GetMedicationByIdQueryHandler>();

            return services;
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorMiddleware>();
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<MedicationDto>, MedicationValidator>();
            services.AddSingleton<IValidator<MedicationFilterDto>, MedicatonFilterValidator>();

            return services;
        }
    }
}
