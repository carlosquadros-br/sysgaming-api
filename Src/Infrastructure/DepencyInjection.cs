using System;
using SysgamingApi.Src.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Infrastructure.Persistence.Repositories;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using SysgamingApi.Src.Application.Users.Token;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Presentation.Services;

namespace SysgamingApi.Src.Infrastructure;

public static class DepencyInjection
{

    public static IServiceCollection AddInfrastructureDI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {

        services.AddScoped<ILoggedInUserService, LoggedInUserService>();

        AddPostgreSQLConnection(services, configuration);

        return services;
    }

    public static void AddPostgreSQLConnection(
      this IServiceCollection services,
      IConfiguration configuration
      )
    {

        services.AddDbContext<AppPostgresDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("PostgreSqlLocal");
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Scoped);

        using var serviceScope = services.BuildServiceProvider().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppPostgresDbContext>();  // Use AppPostgresDbContext for migrations
        context.Database.Migrate();
        context.Database.EnsureCreated();

        services.AddScoped<IAppDbContext>(provider => provider.GetService<AppPostgresDbContext>());

        services.AddScoped<IAccountBalanceRepository, AccountBalanceRepository>();
        services.AddScoped<IBetRepository, BetRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenProvider, UserRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IBaseRepository<Transaction>, TransactionRepository>();
    }
}
