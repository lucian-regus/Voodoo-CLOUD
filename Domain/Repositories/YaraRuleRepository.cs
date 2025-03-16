using Domain.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IYaraRuleRepository : IRepositoryBase<YaraRule>
{
    public Task<List<YaraRule>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime);
}

public class YaraRuleRepository : RepositoryBase<YaraRule>, IYaraRuleRepository
{
    public YaraRuleRepository(DatabaseContext databaseContext) : base(databaseContext) {}
    
    public async Task<List<YaraRule>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime)
    {
        return await Get()
            .Where(s => s.ScrapingLog.Date > scrapingLogTime)
            .ToListAsync();
    }
}