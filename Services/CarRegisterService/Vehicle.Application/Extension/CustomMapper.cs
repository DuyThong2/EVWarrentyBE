using AutoMapper;
using System;
using System.Collections.Generic;
using Vehicle.Application.Dtos;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Models;

namespace Vehicle.Application.Extension
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            // ===== VehiclePart =====
            CreateMap<CreateVehiclePartDto, VehiclePart>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, PartStatus.Installed)))
                .ForMember(d => d.Vehicle, opt => opt.Ignore());

            CreateMap<VehiclePart, VehiclePartDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Vehicle, opt => opt.Ignore()); // tránh vòng lặp mapping

            // ===== Vehicle =====
            CreateMap<CreateVehicleDto, Vehicle.Domain.Models.Vehicle>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, VehicleStatus.Active)))
                .ForMember(d => d.Customer, opt => opt.Ignore());

            CreateMap<Vehicle.Domain.Models.Vehicle, VehicleDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.Parts))
                .ForMember(d => d.Customer, opt => opt.MapFrom(s => s.Customer));

            // ===== Customer =====
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, CustomerStatus.Active)))
                .ForMember(d => d.Vehicles, opt => opt.MapFrom(s => s.Vehicles ?? new List<CreateVehicleDto>()));

            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Vehicles, opt => opt.MapFrom(s => s.Vehicles));

            CreateMap<Customer, CustomerBriefDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            // ===== UpdateCustomerDto =====
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, CustomerStatus.Active)))
                .ForMember(d => d.Vehicles, opt => opt.Ignore()); // Không update vehicles

            // ===== CustomerVehicleDto =====
            CreateMap<Vehicle.Domain.Models.Vehicle, CustomerVehicleDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.Parts));

            // ===== Reverse Maps =====
            CreateMap<VehiclePartDto, VehiclePart>();
            CreateMap<VehicleDto, Vehicle.Domain.Models.Vehicle>();
            CreateMap<UpdateVehicleDto, Vehicle.Domain.Models.Vehicle>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, VehicleStatus.Active)))
                .ForMember(d => d.Customer, opt => opt.Ignore());

            // ===== VehicleImage =====
            CreateMap<CreateVehicleImageDto, VehicleImage>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, VehicleImageStatus.Active)));
            CreateMap<VehicleImage, VehicleImageDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
            CreateMap<VehicleImageDto, VehicleImage>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, VehicleImageStatus.Active)));
            CreateMap<CustomerDto, Customer>();

            // ===== WarrantyHistory =====
            CreateMap<WarrantyHistory, WarrantyHistoryDto>()
                .ForMember(d => d.VIN, opt => opt.MapFrom(s => s.Vehicle.VIN));
        }

        private static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum fallback)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            return Enum.TryParse<TEnum>(value.Trim(), true, out var parsed)
                ? parsed
                : fallback;
        }

        public static WarrantyHistoryDto MapToWarrantyHistoryDto(WarrantyHistory warrantyHistory)
        {
            return new WarrantyHistoryDto
            {
                HistoryId = warrantyHistory.HistoryId,
                VehicleId = warrantyHistory.VehicleId,
                VIN = warrantyHistory.Vehicle?.VIN ?? string.Empty,
                PartId = warrantyHistory.PartId,
                ClaimId = warrantyHistory.ClaimId,
                PolicyId = warrantyHistory.PolicyId,
                EventType = warrantyHistory.EventType.ToString(),
                Description = warrantyHistory.Description,
                PerformedBy = warrantyHistory.PerformedBy,
                ServiceCenterName = warrantyHistory.ServiceCenterName,
                WarrantyStartDate = warrantyHistory.WarrantyStartDate,
                WarrantyEndDate = warrantyHistory.WarrantyEndDate,
                CreatedAt = warrantyHistory.CreatedAt,
                UpdatedAt = warrantyHistory.UpdatedAt,
                Status = warrantyHistory.Status.ToString()
            };
        }
    }
}
