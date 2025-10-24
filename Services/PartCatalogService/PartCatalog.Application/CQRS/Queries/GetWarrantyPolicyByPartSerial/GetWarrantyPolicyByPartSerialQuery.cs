public class GetWarrantyPolicyByPartSerialQuery : IRequest<GetWarrantyPolicyByPartSerialResult>
{
    public string SerialNumber { get; set; }
    public GetWarrantyPolicyByPartSerialQuery(string serialNumber) => SerialNumber = serialNumber;
}

public class GetWarrantyPolicyByPartSerialResult
{
    public int WarrantyDuration { get; set; }
    public int WarrantyDistance { get; set; }
    public DateTime CreatedAt { get; set; }
}