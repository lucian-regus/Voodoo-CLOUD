namespace Infrastructure.DTOs.YaraRule;

public record YaraRuleResponse
{
    public string Rule { get; set; }
    public bool WasRemoved { get; set; }
}