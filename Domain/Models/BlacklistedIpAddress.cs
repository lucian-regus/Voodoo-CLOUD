using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class BlacklistedIpAddress : EntityBase
{
    [MaxLength(39)]
    public string IpAddress { get; set; }
    
    public Guid ScrapingLogId { get; set; }
    public ScrapingLog ScrapingLog { get; set; }
}