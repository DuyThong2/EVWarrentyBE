using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Dtos
{
    public class PartSupplyDto
    {
        public Guid Id { get; set; }
        public Guid ClaimItemId { get; set; }
        public Guid? PartId { get; set; }

        public string ? OldSerialNumber { get; set; }
        public string? Description { get; set; }
        public string? NewSerialNumber { get; set; }
        public string? ShipmentCode { get; set; }
        public string? ShipmentRef { get; set; }
        public string? Status { get; set; }
    }
}
