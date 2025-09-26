using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter
{
    public record GetClaimsFilteredQuery(
       ClaimsFilter Filter,
       PaginationRequest Pagination,
       SortOption Sort,
       IncludeOption Include
   ) : IQuery<GetClaimsFilteredResult>;

    public record GetClaimsFilteredResult(PaginatedResult<ClaimDto> Result);

    public class ClaimsFilter
    {
        public Guid? Id { get; set; }
        public Guid? StaffId { get; set; }
        public string? VIN { get; set; }
        public string? Status { get; set; }      
        public string? ClaimType { get; set; }   
        public Guid? ClaimItemId { get; set; }
        public DateTime? Start { get; set; }     
        public DateTime? End { get; set; }
    }

    public enum SortBy { LastModified, CreatedAt, TotalPrice, Id }
    public enum SortDir { Desc, Asc }

    public record SortOption(SortBy By, SortDir Dir);

    public record IncludeOption(
        bool Technician,            
        bool Items,                 
        bool WorkOrdersWithTech     
    );
}
