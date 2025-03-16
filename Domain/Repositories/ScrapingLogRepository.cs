using Domain.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IScrapingLogRepository : IRepositoryBase<ScrapingLog>
{
    public Task<ScrapingLog> GetByDeltaAsync(DateTime delta);
}

public class ScrapingLogRepository : RepositoryBase<ScrapingLog>, IScrapingLogRepository
{
    public ScrapingLogRepository(DatabaseContext databaseContext) : base(databaseContext) {}
    
    public async Task<ScrapingLog> GetByDeltaAsync(DateTime delta)
    {
        return await Get()
            .FirstOrDefaultAsync(e => e.Date == delta);
    }
}