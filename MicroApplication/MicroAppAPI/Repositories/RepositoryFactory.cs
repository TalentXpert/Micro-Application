using BaseLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MicroAppAPI.Repositories.Interfaces;

namespace MicroAppAPI.Repositories
{

    public class RepositoryFactory : BaseLibraryRepositoryFactory, IRepositoryFactory
    {
        public IUserManagementDatabase UnitOfWork { get; }
        public ILoggerFactory LoggerFactory { get; }
        public RepositoryFactory(IUserManagementDatabase unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
            UnitOfWork = unitOfWork;
            LoggerFactory = loggerFactory;
        }



        IAssetRepository assetRepository;
        public IAssetRepository AssetRepository { get { return assetRepository ?? (assetRepository = new AssetRepository(UnitOfWork, LoggerFactory)); } }

        IPollutionDataRepository pollutionDataRepository;
        public IPollutionDataRepository PollutionDataRepository { get { return pollutionDataRepository ?? (pollutionDataRepository = new PollutionDataRepository(UnitOfWork, LoggerFactory)); } }
    }
}
