using PartCatalog.Application.Data;

public class GetWarrantyPolicyByPartSerialHandler : IRequestHandler<GetWarrantyPolicyByPartSerialQuery, GetWarrantyPolicyByPartSerialResult>
{
    private readonly IApplicationDbContext _context; // hoặc DbContext bạn dùng

    public GetWarrantyPolicyByPartSerialHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetWarrantyPolicyByPartSerialResult> Handle(GetWarrantyPolicyByPartSerialQuery request, CancellationToken cancellationToken)
    {
        // 1. Find Part by SerialNumber
        var part = await _context.Parts.FirstOrDefaultAsync(x => x.SerialNumber == request.SerialNumber, cancellationToken);
        if (part == null)
            throw new KeyNotFoundException("Part not found");

        // 2. Find Package (nếu cần, hoặc Part có liên kết trực tiếp Policy)
        // Ví dụ: var packageId = part.PackageId;
        // 3. Find Policy theo packageId
        var policy = await _context.WarrantyPolicies
            .FirstOrDefaultAsync(x => x.PackageId == part.PackageId, cancellationToken);
        // (Hoặc điều chỉnh theo quan hệ thực tế)

        if (policy == null)
            throw new KeyNotFoundException("WarrantyPolicy not found for Package/Part");

        return new GetWarrantyPolicyByPartSerialResult
        {
            WarrantyDuration = policy.WarrantyDuration ?? 0,
            WarrantyDistance = (int)(policy.WarrantyDistance ?? 0),
            CreatedAt = policy.CreatedAt ?? DateTime.MinValue
        };
    }
}