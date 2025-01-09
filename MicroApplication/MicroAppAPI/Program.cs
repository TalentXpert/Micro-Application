using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BaseLibrary.DatabaseMigrations;
using MicroAppAPI;
using MicroAppAPI.MicroAppConfigurations.DTOs;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<IEntityValidator>(new DataAnnotationsEntityValidator());

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                Array.Empty<string>()
                }
                });
        }
        );

        InitializeApplicationSettings(builder.Configuration);
        UpdateDatabase();
        builder.Services.AddDbContext<UserManagementDatabase>(options => options.UseSqlServer(ApplicationSettings.DatabaseConnectionString));
        builder.Services.AddScoped<IUserManagementDatabase>(provider => provider.GetService<UserManagementDatabase>());

        InitializeApplicationServices(builder.Services);

        //this line resolve dependency injection for AuthOptions anywhere in project. 
        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        #region - authentication 


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = ApplicationSettings.AuthOptions.Issuer,
                ValidAudience = ApplicationSettings.AuthOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationSettings.AuthOptions.SecureKey))
            };
        });
        #endregion
        builder.Services.AddCors(c =>
        {
            c.AddPolicy("AllowAllHeaders",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

        });

        InitializeBaseFactory(builder.Services);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowAllHeaders");
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }

    private static void UpdateDatabase()
    {
        using (var cmdExecutor = new SqlCommandExecutor())
        {
            DatabaseMigrator.UpgradeDatabase(cmdExecutor, ApplicationDatabaseOption.ApplicationDatabase, typeof(Program).Assembly);
        }
    }

    private static void InitializeApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IServiceFactory, ServiceFactory>();
    }
    private static void InitializeBaseFactory(IServiceCollection services)
    {
        services.AddScoped<IBaseLibraryServiceFactory>(GetBaseFactory);
    }
    public static IBaseLibraryServiceFactory GetBaseFactory(IServiceProvider provider)
    {
        var serviceFactory = provider.GetRequiredService<IServiceFactory>();
        serviceFactory.ApplicationControlBaseFactory = new UIControlFactory(serviceFactory);
        serviceFactory.MicroAppContract = new MicroAppContract(serviceFactory);
        IBaseLibraryServiceFactory baseFactory = serviceFactory as BaseLibraryServiceFactory;
        if (baseFactory == null)
            throw new ValidationException("Base service factory can not be null.");
        return baseFactory;
    }
    private static void InitializeApplicationSettings(Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {
        ApplicationSettings.AuthOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
        var applicationSettingConfiguration = configuration.GetSection("ApplicationSettings").Get<ApplicationSettingConfiguration>();
        ApplicationSettings.SetApplicationSettingConfiguration(applicationSettingConfiguration);

        X.Logger = DatabaseLogger.GetDatabaseLogger(ApplicationSettings.DatabaseConnectionString, @"C:\temp");
    }

}

