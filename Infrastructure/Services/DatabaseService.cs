using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.DTOs;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public interface IDatabaseService
{
    public Task<DatabaseUpdateResponse> GetDatabaseUpdatesAsync(DateTime delta);
}

public class DatabaseService : ServiceBase, IDatabaseService
{
    public DatabaseService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<DatabaseUpdateResponse> GetDatabaseUpdatesAsync(DateTime delta)
    {
        DateTime recordsAfter = delta == DateTime.MinValue
            ? DateTime.MinValue
            : delta;
        
        var malwareSignatures = await UnitOfWork.MalwareSignatureRepository.GetAllByScrapingLogTimeAsync(recordsAfter, true);
        var blacklistedIpAddresses = await UnitOfWork.BlacklistedIpAddressRepository.GetAllByScrapingLogTimeAsync(recordsAfter, true);
        var yaraRules = await UnitOfWork.YaraRuleRepository.GetAllByScrapingLogTimeAsync(recordsAfter, true);

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