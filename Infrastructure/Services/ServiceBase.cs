using Domain.Repositories;

namespace Infrastructure.Services;

public interface IServiceBase
{
    IUnitOfWork UnitOfWork { get; }
}

public class ServiceBase : IServiceBase
{
    public IUnitOfWork UnitOfWork { get; }
    
    public ServiceBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}