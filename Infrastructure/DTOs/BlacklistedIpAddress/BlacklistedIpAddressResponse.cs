namespace Infrastructure.DTOs.BlacklistedIpAddress;

public record BlacklistedIpAddressResponse
{
    public string IpAddress { get; set; }
}