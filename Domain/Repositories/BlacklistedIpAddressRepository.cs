using Domain.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IBlacklistedIpAddressRepository : IRepositoryBase<BlacklistedIpAddress>
{
    public Task<List<BlacklistedIpAddress>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime, bool includeDeleted = false);
    public Task<bool> ExistsByIpAsync(string ip);
}


public class BlacklistedIpAddressRepository : RepositoryBase<BlacklistedIpAddress>, IBlacklistedIpAddressRepository
{
    private readonly DatabaseContext _databaseContext;

    public BlacklistedIpAddressRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<BlacklistedIpAddress>> GetAllByScrapingLogTimeAsync(DateTime scrapingLogTime, bool includeDeleted = false)
    {
        return await Get(includeDeleted)
            .Where(s => s.ScrapingLog.Date > scrapingLogTime || s.DeletedAt > scrapingLogTime)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIpAsync(string ip)
    {
        return await Get()
            .AnyAsync(e => e.IpAddress == ip);
    }
}