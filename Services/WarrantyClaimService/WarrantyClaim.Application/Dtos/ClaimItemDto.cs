using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Dtos
{
    public class ClaimItemDto
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public string? PartSerialNumber { get; set; }
        public decimal? PayAmount { get; set; }
        public string? PaidBy { get; set; }
        public string? Note { get; set; }
        public string? ImgURLs { get; set; }
        public string? Status { get; set; }
        public List<PartSupplyDto> PartSupplies { get; set; } = new();
        public List<WorkOrderDto> WorkOrders { get; set; } = new();
    }
}
