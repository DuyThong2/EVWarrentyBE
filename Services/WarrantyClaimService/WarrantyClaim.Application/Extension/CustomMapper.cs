using AutoMapper;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.WarrantyClaim.Queries.GetClaimById
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            CreateMap<Claim, ClaimDto>()
                .ForMember(d => d.ClaimType, opt => opt.MapFrom(s => (int)s.ClaimType))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));

            CreateMap<ClaimItem, ClaimItemDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.WorkOrders, opt => opt.MapFrom(s => s.WorkOrders));

            CreateMap<WorkOrder, WorkOrderDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.Technician, opt => opt.MapFrom(s => s.Technician));

            CreateMap<Technician, TechnicianDto>();
        }
    }
}
