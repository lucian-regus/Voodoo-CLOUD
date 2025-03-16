using Domain.Models;
using Infrastructure.DTOs.YaraRule;

namespace Infrastructure.Mappers;

public static class YaraRuleMapper
{
    public static YaraRuleResponse ToYaraRuleResponse(this YaraRule yaraRule)
    {
        var response = new YaraRuleResponse();
        response.Rule = yaraRule.Rule;
        
        return response;
    } 
}