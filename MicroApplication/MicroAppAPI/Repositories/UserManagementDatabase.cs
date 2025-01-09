using BaseLibrary.Database;

using Microsoft.EntityFrameworkCore;
using MicroAppAPI.Repositories.Interfaces;



namespace MicroAppAPI.Repositories
{
    public class UserManagementDatabase : BaseDatabase, IUserManagementDatabase
    {
        public static UserManagementDatabase GetDatabase(string databaseConnectionString)
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(databaseConnectionString).Options;
            var database = new UserManagementDatabase(dbContextOptions, new DataAnnotationsEntityValidator());
            return database;
        }

        public UserManagementDatabase(DbContextOptions options, IEntityValidator entityValidator) : base(options, entityValidator)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (AppSettings.Environment != "Production")
            //    optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Asset>().ToTable("Asset").HasKey(x => x.Id);
            modelBuilder.Entity<PollutionData>().ToTable("PollutionData").HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }



        DbSet<Asset> assets;
        public DbSet<Asset> Assets { get { return assets ?? (assets = Set<Asset>()); } }
        DbSet<PollutionData> pollutionData;
        public DbSet<PollutionData> PollutionData { get { return pollutionData ?? (pollutionData = Set<PollutionData>()); } }
    }
}
