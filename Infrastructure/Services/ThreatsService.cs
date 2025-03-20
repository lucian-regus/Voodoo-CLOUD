using Domain.Repositories;
using Infrastructure.DTOs;

namespace Infrastructure.Services;

public interface IThreatsService
{
    public Task<ExistsResponse> CheckIpExistsAsync(string ip);
    public Task<ExistsResponse> CheckSignatureExistsAsync(string hash);
}

public class ThreatsService : ServiceBase, IThreatsService
{
    public ThreatsService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<ExistsResponse> CheckIpExistsAsync(string ip)
    {
        var exists = await UnitOfWork.BlacklistedIpAddressRepository.ExistsByIpAsync(ip);

        return new ExistsResponse
        {
            Exists = exists
        };
    }

    public async Task<ExistsResponse> CheckSignatureExistsAsync(string hash)
    {
        var exists = await UnitOfWork.MalwareSignatureRepository.ExistsByHashAsync(hash);

        return new ExistsResponse
        {
            Exists = exists
        };
    }
}