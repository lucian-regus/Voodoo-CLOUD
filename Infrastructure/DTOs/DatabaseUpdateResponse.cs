using Infrastructure.DTOs.BlacklistedIpAddress;
using Infrastructure.DTOs.MalwareSignature;
using Infrastructure.DTOs.YaraRule;

namespace Infrastructure.DTOs;

public record DatabaseUpdateResponse
{
    public List<BlacklistedIpAddressResponse> BlacklistedIpAddresses { get; set; } = [];
    public List<YaraRuleResponse> YaraRules { get; set; } = [];
    public List<MalwareSignatureResponse> MalwareSignatures { get; set; } = [];
}