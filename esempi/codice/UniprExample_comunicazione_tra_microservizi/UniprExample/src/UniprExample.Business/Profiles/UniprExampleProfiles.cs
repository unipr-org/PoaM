using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using UniprExample.Repository.Model;
using UniprExample.Shared.Dto;

namespace UniprExample.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker {
    AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile {
    public InputFileProfile() {
        CreateMap<StudenteDto, Studenti>();
        CreateMap<StudenteInsertDto, Studenti>();
        CreateMap<Studenti, StudenteDto>();

        CreateMap<Studenti, EsameStudenteDto>();
        CreateMap<Corsi, EsameStudenteDto>()
            .ForMember(dest => dest.NomeCorso, opt => opt.MapFrom(src => src.Titolo));
        CreateMap<Esami, EsameStudenteDto>();
    }
}