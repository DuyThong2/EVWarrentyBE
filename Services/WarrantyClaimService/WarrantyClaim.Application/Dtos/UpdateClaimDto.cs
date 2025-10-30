using System;
using System.Collections.Generic;

namespace WarrantyClaim.Application.Dtos
{
    public class UpdateClaimDto
    {
        public Guid Id { get; set; }
        public Guid? StaffId { get; set; }
        public string VIN { get; set; } = string.Empty;
        public long? DistanceMeter { get; set; }
        public string? Description { get; set; }
        public string? FileURL { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? ClaimType { get; set; }
        public string? Status { get; set; }

        public List<UpdateClaimItemDto> Items { get; set; } = new();
    }
}
