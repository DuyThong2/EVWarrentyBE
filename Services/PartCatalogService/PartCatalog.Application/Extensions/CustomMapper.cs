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

        CreateMap<CreatePartDto, Part>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                s.Status == null
                    ? ActiveStatus.ACTIVE   // fallback mặc định
                    : ParseEnumOrDefault(s.Status, ActiveStatus.ACTIVE)))
            .ForMember(d => d.Category, opt => opt.Ignore())
            .ForMember(d => d.Package, opt => opt.Ignore());

        CreateMap<Part, PartDto>().ReverseMap();
        // ===== Package =====
        CreateMap<Package, PackageDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.PartId, opt => opt.Ignore())
            .ForMember(d => d.PartName, opt => opt.Ignore());

        CreateMap<PackageDto, Package>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Status)
                    ? ActiveStatus.ACTIVE   // fallback mặc định
                    : ParseEnumOrDefault(s.Status, ActiveStatus.ACTIVE)))
            .ForMember(d => d.Parts, opt => opt.Ignore())
            .ForMember(d => d.WarrantyPolicies, opt => opt.Ignore());

        CreateMap<Package, PackageDto>().ReverseMap();


        // ===== Category ====
        //CreateMap<Category, CategoryDto>().ReverseMap();
        // ===== WarrantyPolicy =====
        CreateMap<CreateWarrantyPolicyDto, WarrantyPolicy>()
            .ForMember(d => d.PolicyId, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

        CreateMap<WarrantyPolicy, WarrantyPolicyDto>()
            .ForMember(d => d.PackageName, opt => opt.MapFrom(s => s.Package != null ? s.Package.Name : null));

        CreateMap<WarrantyPolicy, WarrantyPolicyDto>().ReverseMap();
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
