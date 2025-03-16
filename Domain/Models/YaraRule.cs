namespace Domain.Models;

public class YaraRule : EntityBase
{
    // ADD MAXLENGTH HERE
    public string Rule { get; set; }
    
    public Guid ScrapingLogId { get; set; }
    public ScrapingLog ScrapingLog { get; set; }
}