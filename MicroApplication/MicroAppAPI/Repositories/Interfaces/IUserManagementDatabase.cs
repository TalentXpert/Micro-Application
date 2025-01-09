using BaseLibrary.Database;

using Microsoft.EntityFrameworkCore;



namespace MicroAppAPI.Repositories.Interfaces
{
    public interface IUserManagementDatabase : IBaseDatabase
    {

        DbSet<Asset> Assets { get; }
        DbSet<PollutionData> PollutionData { get; }

    }
}
