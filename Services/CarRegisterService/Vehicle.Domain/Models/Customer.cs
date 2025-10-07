using System;
using System.Collections.Generic;


namespace Vehicle.Domain.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public CustomerStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Quan hệ 1-nhiều → Vehicles
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
