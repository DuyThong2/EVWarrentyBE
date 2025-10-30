using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Domain.Models
{
    public class ClaimItem : Entity<Guid>
    {
        public Guid ClaimId { get; set; }
        public string? PartSerialNumber { get; set; }

        public string? VIN { get; set; } 

        public decimal? PayAmount { get; set; }
        public string? PaidBy { get; set; }         
        public string? Note { get; set; }
        public string? ImgURLs { get; set; }
        public ClaimItemStatus Status { get; set; }          

        // Navs
        public Claim Claim { get; set; } = null!;

        public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

        public ICollection<PartSupply> PartSupplies { get; set; } = new List<PartSupply>();
    }
}
