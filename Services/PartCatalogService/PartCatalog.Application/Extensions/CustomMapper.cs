using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PartCatalog.Application.DTOs;
using PartCatalog.Domain.Models;
using PartCatalog.Domain.Enums;
using PartCatalog.Application.CQRS.Queries.GetPartById;

namespace PartCatalog.Application.Extensions;

public class CustomMapper : Profile
{
    public CustomMapper()
    {
        // ===== Part =====
        CreateMap<Part, PartDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
        .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category))
        .ForMember(d => d.Package, opt => opt.MapFrom(s => s.Package));

        CreateMap<PartDto, Part>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Status)
                    ? ActiveStatus.ACTIVE   // fallback mặc định
                    : ParseEnumOrDefault(s.Status, ActiveStatus.ACTIVE)))
        .ForMember(d => d.Category, opt => opt.Ignore())
        .ForMember(d => d.Package, opt => opt.Ignore());


        // ===== Category =====
        //CreateMap<Category, CategoryDto>().ReverseMap();

        // ===== Package =====
        //CreateMap<Package, PackageDto>().ReverseMap();

        // ===== WarrantyPolicy =====
        //CreateMap<WarrantyPolicy, WarrantyPolicyDto>().ReverseMap();
    }

    static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum fallback)
            where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        return Enum.TryParse<TEnum>(value.Trim(), true, out var parsed)
            ? parsed
            : fallback;
    }
}
