using Domain.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IYaraRuleRepository : IRepositoryBase<YaraRule>
{
    public Task<List<YaraRule>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime, bool includeDeleted = false);
}

public class YaraRuleRepository : RepositoryBase<YaraRule>, IYaraRuleRepository
{
    public YaraRuleRepository(DatabaseContext databaseContext) : base(databaseContext) {}
    
    public async Task<List<YaraRule>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime, bool includeDeleted = false)
    {
        return await Get(includeDeleted)
            .Where(s => s.ScrapingLog.Date > scrapingLogTime || s.DeletedAt > scrapingLogTime)
            .ToListAsync();
    }
}