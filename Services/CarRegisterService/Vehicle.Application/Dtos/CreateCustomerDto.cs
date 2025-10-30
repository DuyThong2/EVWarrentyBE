using System;
using System.Collections.Generic;
using Vehicle.Domain.Enums;

namespace Vehicle.Application.Dtos
{
    public class CreateCustomerDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Status { get; set; }
        public List<CreateVehicleDto> Vehicles { get; set; } = new();
    }
}
