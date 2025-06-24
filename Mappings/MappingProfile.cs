using AutoMapper;
using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Asset DTOs
        CreateMap<Asset, AssetDto>().ReverseMap();
        CreateMap<AssetAddDto, Asset>()
            .ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(src => Enum.Parse<EnergyType>(src.Type, true))
            ) // Asset energy tyoe should never change
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => Enum.Parse<AssetStatus>(src.Status, true))
            )
            .ForMember(dest => dest.Metrics, opt => opt.Ignore()); // Metrics should remain untouched during asset update

        CreateMap<AssetPutDto, Asset>()
            .ForMember(
                dest => dest.Name,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name))
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => Enum.Parse<AssetStatus>(src.Status, true))
            );

        // MetricData DTOs
        CreateMap<MetricData, MetricDataDto>()
            .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name));
        CreateMap<MetricDataAddDto, MetricData>().ReverseMap();

        // User DTOs
        CreateMap<User, UserDto>();
        CreateMap<UserLoginDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
