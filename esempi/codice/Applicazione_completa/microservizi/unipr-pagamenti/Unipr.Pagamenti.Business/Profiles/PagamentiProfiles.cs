using AutoMapper;
using Pagamenti.Repository.Model;
using Pagamenti.Shared;
using System.Diagnostics.CodeAnalysis;

namespace Pagamenti.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker {
    AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile {
    public InputFileProfile() {
        CreateMap<VersamentoInsertDto, Versamento>();
        CreateMap<Versamento, VersamentoInsertDto>();
        CreateMap<VersamentoReadDto, Versamento>();
        CreateMap<Versamento, VersamentoReadDto>();
    }
}