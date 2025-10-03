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
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.Parts ?? new List<CreateVehiclePartDto>()))
                .ForMember(d => d.Customer, opt => opt.Ignore());

            CreateMap<Vehicle.Domain.Models.Vehicle, VehicleDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.Parts))
                .ForMember(d => d.Customer, opt => opt.Ignore());

            // ===== Customer =====
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s =>
                    ParseEnumOrDefault(s.Status, CustomerStatus.Active)))
                .ForMember(d => d.Vehicles, opt => opt.MapFrom(s => s.Vehicles ?? new List<CreateVehicleDto>()));

            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Vehicles, opt => opt.MapFrom(s => s.Vehicles));

            // ===== Reverse Maps =====
            CreateMap<VehiclePartDto, VehiclePart>();
            CreateMap<VehicleDto, Vehicle.Domain.Models.Vehicle>();
            CreateMap<CustomerDto, Customer>();
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
    }
}
