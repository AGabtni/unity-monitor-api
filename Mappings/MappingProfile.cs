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
            .ForMember(dest => dest.Type, opt => opt.Ignore()) // Asset energy tyoe should never change
            .ForMember(dest => dest.Metrics, opt => opt.Ignore()); // Metrics should remain untouched during asset update

        // MetricData DTOs
        CreateMap<MetricData, MetricDataDto>()
            .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name));
        CreateMap<MetricDataAddDto, MetricData>().ReverseMap();
    }
}
