namespace MedicationApi.Application.Mappers
{
    using AutoMapper;
    using MedicationApi.Contracts;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;

    public class MedicationMapper : Profile
    {
        public MedicationMapper()
        {
            CreateMap<Medication, MedicationDto>()
                .ReverseMap();

            CreateMap<MedicationDto, CreateMedicationCommand>()
                .ForMember(c => c.Medication, o => o.MapFrom(dto => dto));

            CreateMap<MedicationFilterDto, GetMedicationsQuery>()
                .ReverseMap();
        }
    }
}
