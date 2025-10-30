using System;
using System.Collections.Generic;

namespace Vehicle.Application.Dtos
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<CustomerVehicleDto> Vehicles { get; set; } = new();
    }
}


