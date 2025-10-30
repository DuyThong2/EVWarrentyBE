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
        // ===== ClaimItem (Create) =====
        CreateMap<CreateClaimItemDto, ClaimItem>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, ClaimItemStatus.PENDING)))
            .ForMember(d => d.Claim, opt => opt.Ignore())
            .ForMember(d => d.WorkOrders, opt => opt.Ignore())
            .ForMember(d => d.PartSupplies, opt => opt.Ignore());

        // ===== Claim (Create) =====
        CreateMap<CreateClaimDto, Claim>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.ClaimType, ClaimType.WARRANT)))
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, ClaimStatus.SUBMITTED)))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items ?? new List<CreateClaimItemDto>()));

        // ===== Technician =====
        CreateMap<Technician, TechnicianDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        // DTO → Entity
        CreateMap<TechnicianDto, Technician>()
            .ForMember(d => d.WorkOrders, opt => opt.Ignore())
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, TechnicianStatus.ACTIVE)));

        // ===== WorkOrder =====
        CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Technician, opt => opt.MapFrom(s => s.Technician));

        CreateMap<WorkOrderDto, WorkOrder>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, WorkOrderStatus.OPEN)))
            .ForMember(d => d.Technician, opt => opt.Ignore());

        // ===== Claim -> ClaimDto =====
        CreateMap<Claim, ClaimDto>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s => s.ClaimType.ToString()))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));

        // ===== ClaimDto -> Claim =====
        CreateMap<ClaimDto, Claim>()
            .ForMember(d => d.ClaimType, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.ClaimType, ClaimType.WARRANT)))
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, ClaimStatus.SUBMITTED)))
            .ForMember(d => d.Items, opt => opt.Ignore())
            .ForMember(d => d.FileURL, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.LastModified, opt => opt.Ignore())
            ;
        

        // ===== ClaimItem <-> ClaimItemDto =====
        CreateMap<ClaimItem, ClaimItemDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.WorkOrders, opt => opt.MapFrom(s => s.WorkOrders));

        CreateMap<ClaimItemDto, ClaimItem>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                EnumParser.ParseOrDefault(s.Status, ClaimItemStatus.PENDING)))
            .ForMember(d => d.Claim, opt => opt.Ignore())
            .ForMember(d => d.WorkOrders, opt => opt.Ignore())
            .ForMember(d => d.PartSupplies, opt => opt.Ignore())
            .ForMember(d => d.ImgURLs, opt => opt.Ignore());


        CreateMap<PartSupply, PartSupplyDto>();

        CreateMap<UpdateClaimDto, Claim>()
            .ForMember(d => d.Items, opt => opt.Ignore());

        CreateMap<UpdateClaimItemDto, ClaimItem>()
            .ForMember(d => d.WorkOrders, opt => opt.Ignore())
            .ForMember(d => d.PartSupplies, opt => opt.Ignore());

    }


}
