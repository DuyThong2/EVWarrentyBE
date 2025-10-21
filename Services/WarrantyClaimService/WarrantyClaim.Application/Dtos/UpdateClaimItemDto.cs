using System;

namespace WarrantyClaim.Application.Dtos
{
    public class UpdateClaimItemDto
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public string? PartSerialNumber { get; set; }
        public decimal? PayAmount { get; set; }
        public string? PaidBy { get; set; }
        public string? Note { get; set; }
        public string? ImgURLs { get; set; }
        public string? Status { get; set; }
        
    }
}
