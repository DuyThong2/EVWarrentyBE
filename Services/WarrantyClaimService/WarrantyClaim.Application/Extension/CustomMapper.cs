using AutoMapper;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById;
using WarrantyClaim.Domain.Enums;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.Extension;

public class CustomMapper : Profile
{

    public CustomMapper()
    {
        CreateMap<CreateClaimItemDto, ClaimItem>()
             .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                 ParseEnumOrDefault(s.Status, ClaimItemStatus.PENDING)))
             .ForMember(d => d.Claim, opt => opt.Ignore())
             .ForMember(d => d.WorkOrders, opt => opt.Ignore())
             .ForMember(d => d.PartSupplies, opt => opt.Ignore());

        // Claim
        CreateMap<CreateClaimDto, Claim>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s =>
                ParseEnumOrDefault(s.ClaimType, ClaimType.WARRANT)))
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                ParseEnumOrDefault(s.Status, ClaimStatus.SUBMITTED)))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items ?? new List<CreateClaimItemDto>()));
    

    // ===== WorkOrder =====
    CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(d => d.Technician, opt => opt.MapFrom(s => s.Technician)); 

        CreateMap<WorkOrderDto, WorkOrder>()
            .ForMember(d => d.Technician, opt => opt.Ignore());

        // ===== Technician =====
        CreateMap<Technician, TechnicianDto>().ReverseMap();

        CreateMap<Claim, ClaimDto>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s => s.ClaimType.ToString()))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));

        // ===== ClaimItem =====
        CreateMap<ClaimItem, ClaimItemDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.WorkOrders, opt => opt.MapFrom(s => s.WorkOrders));

        // ===== WorkOrder =====
        CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Technician, opt => opt.MapFrom(s => s.Technician));

        // ===== Claim =====
        CreateMap<ClaimDto, Claim>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.ClaimType)
                    ? ClaimType.WARRANT
                    : (ClaimType)Enum.Parse(typeof(ClaimType), s.ClaimType, true)))
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Status)
                    ? ClaimStatus.SUBMITTED
                    : (ClaimStatus)Enum.Parse(typeof(ClaimStatus), s.Status, true)))
            .ForMember(d => d.Items, opt => opt.Ignore());

        // ===== ClaimItem =====
        CreateMap<ClaimItemDto, ClaimItem>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Status)
                    ? ClaimItemStatus.PENDING
                    : (ClaimItemStatus)Enum.Parse(typeof(ClaimItemStatus), s.Status, true)))
            .ForMember(d => d.Claim, opt => opt.Ignore())
            .ForMember(d => d.WorkOrders, opt => opt.Ignore())
            .ForMember(d => d.PartSupplies, opt => opt.Ignore());

        // ===== WorkOrder =====
        CreateMap<WorkOrderDto, WorkOrder>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Status)
                    ? WorkOrderStatus.OPEN
                    : (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), s.Status, true)))
            .ForMember(d => d.Technician, opt => opt.Ignore());
    }
    //public CustomMapper()
    //{
    //    CreateMap<Claim, ClaimDto>()
    //        .ForMember(d => d.ClaimType, opt => opt.MapFrom(s => (int)s.ClaimType))
    //        .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
    //        .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));

    //    CreateMap<ClaimItem, ClaimItemDto>()
    //        .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
    //        .ForMember(d => d.WorkOrders, opt => opt.MapFrom(s => s.WorkOrders));

    //    CreateMap<WorkOrder, WorkOrderDto>()
    //        .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
    //        .ForMember(d => d.Technician, opt => opt.MapFrom(s => s.Technician));

    //    CreateMap<Technician, TechnicianDto>();
    //}

    static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum fallback)
            where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        return Enum.TryParse<TEnum>(value.Trim(), true, out var parsed)
            ? parsed
            : fallback;
    }
}
