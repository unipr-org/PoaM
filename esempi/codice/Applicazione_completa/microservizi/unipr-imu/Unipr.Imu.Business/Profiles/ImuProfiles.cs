using AutoMapper;
using Imu.Repository.Model;
using Pagamenti.Shared;
using System.Diagnostics.CodeAnalysis;

namespace Imu.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker {
    AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile {
    public InputFileProfile() {
        CreateMap<VersamentoReadDto, VersamentoKafka>();
        CreateMap<VersamentoKafka, VersamentoReadDto>();
    }
}
