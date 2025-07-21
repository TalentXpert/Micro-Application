using BaseLibrary.Domain.Audit;
using BaseLibrary.Domain.DataSources;
using BaseLibrary.Domain.PerformanceLogs;
using Microsoft.EntityFrameworkCore;

namespace BaseLibrary.Database
{
    public interface IBaseDatabase : IQueryableUnitOfWork
    {
        DbSet<ApplicationUser> ApplicationUsers { get; }
        DbSet<ApplicationUserSetting> UserSettings { get; }
        DbSet<FormDataStore> PageDataStore { get; }
        DbSet<AppPage> AppPages { get; }
        DbSet<AppForm> AppForms { get; }
        DbSet<AppControl> AppControls { get; }
        DbSet<AppFormControl> AppFormControls { get; }
        DbSet<ApplicationSetting> ApplicationSettings { get; }
        DbSet<ComponentSchema> ComponentSchemas { get; }
        DbSet<SqlDataSource> SQLDataSources { get; }
        DbSet<ApplicationRole> Role { get; }
        DbSet<Permission> Permission { get; }
        DbSet<UserRole> UserRole { get; }

        DbSet<ApplicationRolePermission> RolePermission { get; }
        DbSet<Organization> Organizations { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<PerformanceLog> PerformanceLogs { get; }
    }

    public class BaseDatabase : QueryableUnitOfWork, IBaseDatabase
    {
        public BaseDatabase(DbContextOptions options, IEntityValidator entityValidator) : base(options, entityValidator)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ApplicationSettingBase.Environment != "Production")
                optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser").HasKey(x => x.Id);
            modelBuilder.Entity<ApplicationRole>().ToTable("ApplicationRole").HasKey(x => x.Id);
            modelBuilder.Entity<Permission>().ToTable("Permission").HasKey(x => x.Id);
            modelBuilder.Entity<UserRole>().ToTable("UserRole").HasKey(x => x.Id);
            modelBuilder.Entity<ApplicationRolePermission>().ToTable("ApplicationRolePermission").HasKey(x => x.Id);
            
            modelBuilder.Entity<ApplicationUserSetting>().ToTable("ApplicationUserSetting").HasKey(x => x.Id);
            modelBuilder.Entity<FormDataStore>().ToTable("FormDataStore").HasKey(x => x.Id);

            modelBuilder.Entity<AppPage>().ToTable("AppPage").HasKey(x => x.Id);

            modelBuilder.Entity<AppForm>().ToTable("AppForm").HasKey(x => x.Id);
            modelBuilder.Entity<AppForm>().HasMany(p => p.AppFormControls).WithOne(f => f.AppForm).HasForeignKey(f => f.AppFormId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppControl>().ToTable("AppControl").HasKey(x => x.Id);
            modelBuilder.Entity<AppControl>().HasMany(p => p.AppFormControls).WithOne(f => f.AppControl).HasForeignKey(f => f.AppControlId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppFormControl>().ToTable("AppFormControl").HasKey(x => x.Id);
            modelBuilder.Entity<AppFormControl>().HasOne(fc => fc.AppForm).WithMany(p => p.AppFormControls).HasForeignKey(fc => fc.AppFormId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AppFormControl>().HasOne(fc => fc.AppControl).WithMany(p => p.AppFormControls).HasForeignKey(fc => fc.AppControlId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationSetting>().ToTable("ApplicationSetting").HasKey(x => x.Id);
            modelBuilder.Entity<ComponentSchema>().ToTable("ComponentSchema").HasKey(x => x.Id);
            modelBuilder.Entity<SqlDataSource>().ToTable("SQLDataSource").HasKey(x => x.Id);
            modelBuilder.Entity<Organization>().ToTable("Organization").HasKey(x => x.Id);
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog").HasKey(x => x.Id);
            modelBuilder.Entity<PerformanceLog>().ToTable("PerformanceLog").HasKey(x => x.Id);
            
        }
        DbSet<Permission> permission;
        public DbSet<Permission> Permission { get { return permission ?? (permission = Set<Permission>()); } }

        DbSet<ApplicationRole> role;
        public DbSet<ApplicationRole> Role { get { return role ?? (role = Set<ApplicationRole>()); } }

        DbSet<UserRole> user_role;
        public DbSet<UserRole> UserRole { get { return user_role ?? (user_role = Set<UserRole>()); } }

        DbSet<ApplicationRolePermission> rolePermission;
        public DbSet<ApplicationRolePermission> RolePermission { get { return rolePermission ?? (rolePermission = Set<ApplicationRolePermission>()); } }

        DbSet<ApplicationUser> applicationUsers;
        public DbSet<ApplicationUser> ApplicationUsers { get { return applicationUsers ?? (applicationUsers = Set<ApplicationUser>()); } }

        DbSet<ApplicationSetting>? applicationSettings;
        public DbSet<ApplicationSetting> ApplicationSettings { get { return applicationSettings ?? (applicationSettings = Set<ApplicationSetting>()); } }

        DbSet<ApplicationUserSetting>? userSettings;
        public DbSet<ApplicationUserSetting> UserSettings { get { return userSettings ?? (userSettings = Set<ApplicationUserSetting>()); } }


        DbSet<FormDataStore>? pageData;
        public DbSet<FormDataStore> PageDataStore { get { return pageData ?? (pageData = Set<FormDataStore>()); } }

        DbSet<AppPage>? appPages;
        public DbSet<AppPage> AppPages { get { return appPages ?? (appPages = Set<AppPage>()); } }

        DbSet<AppForm>? appForm;
        public DbSet<AppForm> AppForms { get { return appForm ?? (appForm = Set<AppForm>()); } }

        DbSet<AppControl>? appControls;
        public DbSet<AppControl> AppControls { get { return appControls ?? (appControls = Set<AppControl>()); } }

        DbSet<AppFormControl>? appFormControls;
        public DbSet<AppFormControl> AppFormControls { get { return appFormControls ?? (appFormControls = Set<AppFormControl>()); } }

        DbSet<ComponentSchema>? componentSchemas;
        public DbSet<ComponentSchema> ComponentSchemas { get { return componentSchemas ?? (componentSchemas = Set<ComponentSchema>()); } }
        
        DbSet<SqlDataSource>? sQLDataSources;
        public DbSet<SqlDataSource> SQLDataSources { get { return sQLDataSources ?? (sQLDataSources = Set<SqlDataSource>()); } }

        DbSet<Organization>? organization;
        public DbSet<Organization> Organizations { get { return organization ?? (organization = Set<Organization>()); } }

        DbSet<AuditLog>? auditLog;
        public DbSet<AuditLog> AuditLogs { get { return auditLog ??= Set<AuditLog>(); } }

        DbSet<PerformanceLog>? performanceLog;
        public DbSet<PerformanceLog> PerformanceLogs { get { return performanceLog ??= Set<PerformanceLog>(); } }

        
    }
}
