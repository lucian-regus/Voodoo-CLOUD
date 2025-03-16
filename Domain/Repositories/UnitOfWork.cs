using Domain.Database;

namespace Domain.Repositories;

public interface IUnitOfWork
{
    IBlacklistedIpAddressRepository BlacklistedIpAddressRepository { get; }
    IMalwareSignatureRepository MalwareSignatureRepository { get; }
    IYaraRuleRepository YaraRuleRepository { get; }
    IScrapingLogRepository ScrapingLogRepository { get; }
    
    DatabaseContext DatabaseContext { get; }
    Task<bool> SaveChangesAsync();
}

public class UnitOfWork :  IUnitOfWork
{
    public IBlacklistedIpAddressRepository BlacklistedIpAddressRepository { get; }
    public IMalwareSignatureRepository MalwareSignatureRepository { get; }
    public IYaraRuleRepository YaraRuleRepository { get; }
    public IScrapingLogRepository ScrapingLogRepository { get; }
    
    public DatabaseContext DatabaseContext { get; }
    
    public UnitOfWork(DatabaseContext databaseContext,
        IBlacklistedIpAddressRepository blacklistedIpAddressRepository,
        IMalwareSignatureRepository malwareSignatureRepository,
        IYaraRuleRepository yaraRuleRepository,
        IScrapingLogRepository scrapingLogRepository)
    {
        DatabaseContext = databaseContext;
        
        BlacklistedIpAddressRepository = blacklistedIpAddressRepository;
        MalwareSignatureRepository = malwareSignatureRepository;
        YaraRuleRepository = yaraRuleRepository;
        ScrapingLogRepository = scrapingLogRepository;
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return await DatabaseContext.SaveChangesAsync() > 0;
    }
}