using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.DTOs;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public interface IDatabaseService
{
    public Task<DatabaseUpdateResponse> GetDatabaseUpdatesAsync(DateTime delta);
}

public class DatabaseService: ServiceBase, IDatabaseService
{
    public DatabaseService(IUnitOfWork unitOfWork) : base(unitOfWork) {}

    public async Task<DatabaseUpdateResponse> GetDatabaseUpdatesAsync(DateTime delta)
    {
        var scrapingLog = await UnitOfWork.ScrapingLogRepository.GetByDeltaAsync(delta);
        if (scrapingLog == null)
            throw new BadRequestException("Invalid delta value.");
        
        var malwareSignatures = await UnitOfWork.MalwareSignatureRepository.GetAllByScrapingLogTimeAsync(scrapingLog.Date);
        var blacklistedIpAddresses = await UnitOfWork.BlacklistedIpAddressRepository.GetAllByScrapingLogTimeAsync(scrapingLog.Date);
        var yaraRules = await UnitOfWork.YaraRuleRepository.GetAllByScrapingLogTimeAsync(scrapingLog.Date);

        var malwareSignaturesResponse = malwareSignatures
            .Select(e => e.ToMalwareSignatureResponse())
            .ToList();
        
        var blacklistedIpAddressesResponse = blacklistedIpAddresses
            .Select(e => e.ToBlacklistedIpAddressResponse())
            .ToList();
        
        var yaraRulesResponse = yaraRules
            .Select(e => e.ToYaraRuleResponse())
            .ToList();

        var databaseUpdateResponse = new DatabaseUpdateResponse();
        databaseUpdateResponse.MalwareSignatures = malwareSignaturesResponse;
        databaseUpdateResponse.BlacklistedIpAddresses = blacklistedIpAddressesResponse;
        databaseUpdateResponse.YaraRules = yaraRulesResponse;

        return databaseUpdateResponse;
    }
}