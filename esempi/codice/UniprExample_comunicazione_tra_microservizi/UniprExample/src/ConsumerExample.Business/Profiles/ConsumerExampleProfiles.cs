using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using ConsumerExample.Repository.Model;
using ConsumerExample.Shared.Dto;
using UniprExample.Shared.Dto;

namespace ConsumerExample.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker {
    AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile {
    public InputFileProfile() {
        CreateMap<StudentiKafkaDto, StudentiKafka>();
        CreateMap<StudentiKafka, StudentiKafkaDto>();
        CreateMap<StudenteDto, StudentiKafka>();
    }
}
