using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Domain.Models
{
    public class PartSupply : Entity<Guid>
    {
        public Guid? ClaimItemId { get; set; }
        public Guid? PartId { get; set; }

        public string? OldPartSerialNumber { get; set; }

        public string? Description { get; set; }
        public string? NewSerialNumber { get; set; } // varchar(100)
        public string? ShipmentCode { get; set; }    // varchar(50)
        public string? ShipmentRef { get; set; }     // varchar(80)
        public SupplyStatus Status { get; set; }

        // Navs
        public ClaimItem ? ClaimItem { get; set; } = null!;
    }
}
