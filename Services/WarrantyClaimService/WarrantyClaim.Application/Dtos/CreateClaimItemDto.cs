using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Application.Dtos
{
    public class CreateClaimDto
    {
        public Guid? StaffId { get; set; }
        public string VIN { get; set; } = null!;
        public long? DistanceMeter { get; set; }
        public string? Description { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? ClaimType { get; set; }
        public string? Status { get; set; }

        public List<CreateClaimItemDto> Items { get; set; } = new();
    }

    public class CreateClaimItemDto
    {
        public string? PartSerialNumber { get; set; }
        public decimal? PayAmount { get; set; }
        public string? PaidBy { get; set; }
        public string? Note { get; set; }
        public string? ImgURLs { get; set; }
        public string? Status { get; set; }
    }
}
