using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Domain.Models
{
    public class Claim : Aggregate<Guid>   // CreatedAt/LastModified kế thừa từ Entity<T>
    {
        public Guid? StaffId { get; set; }
        public string VIN { get; set; } = null!;
        public long? DistanceMeter { get; set; }
        public string? Description { get; set; }
        public string? FileURL { get; set; }
        public decimal? TotalPrice { get; set; }
        public ClaimType ClaimType { get; set; }
        public ClaimStatus Status { get; set; }

        // Navs
        public ICollection<ClaimItem> Items { get; set; } = new List<ClaimItem>();
        //public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
    }

}
