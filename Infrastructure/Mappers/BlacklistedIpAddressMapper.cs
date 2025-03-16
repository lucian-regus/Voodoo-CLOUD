using Domain.Models;
using Infrastructure.DTOs.BlacklistedIpAddress;
using Infrastructure.DTOs.MalwareSignature;

namespace Infrastructure.Mappers;

public static class BlacklistedIpAddressMapper
{
    public static BlacklistedIpAddressResponse ToBlacklistedIpAddressResponse(this BlacklistedIpAddress blacklistedIpAddress)
    {
        var response = new BlacklistedIpAddressResponse();
        response.IpAddress = blacklistedIpAddress.IpAddress;
        
        return response;
    }
}