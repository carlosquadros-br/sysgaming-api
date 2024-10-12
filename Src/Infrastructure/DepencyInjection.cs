using System;
using SysgamingApi.Src.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Infrastructure.Persistence.Repositories;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace SysgamingApi.Src.Infrastructure;

public static class DepencyInjection
{

    public static IServiceCollection AddInfrastructureDI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {

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
            });

        using var serviceScope = services.BuildServiceProvider().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppPostgresDbContext>();  // Use AppPostgresDbContext for migrations
        context.Database.Migrate();
        context.Database.EnsureCreated();

        services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppPostgresDbContext>()
        .AddDefaultTokenProviders();


        services.AddScoped<IAppDbContext>(provider => provider.GetService<AppPostgresDbContext>());



        services.AddScoped<IBetRepository, BetRepository>();
    }

}
