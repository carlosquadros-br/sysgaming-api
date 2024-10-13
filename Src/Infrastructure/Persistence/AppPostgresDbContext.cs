using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;

namespace SysgamingApi.Src.Infrastructure.Persistence;

public class AppPostgresDbContext : IdentityDbContext<User>, IAppDbContext
{

    private readonly ILoggedInUserService? _loggedInUserService;

    public AppPostgresDbContext(DbContextOptions<AppPostgresDbContext> options) : base(options)
    {
    }

    public AppPostgresDbContext(DbContextOptions<AppPostgresDbContext> options, ILoggedInUserService loggedInUserService) : base(options)
    {
        _loggedInUserService = loggedInUserService;
    }

    // Implement the generic Set<TEntity> method from IAppDbContext
    public override DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>(); // Use the base DbContext's Set method
    }

    // Ensure the SaveChangesAsync method is implemented
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_loggedInUserService == null)
        {
            throw new Exception("Logged in user service is null");
        }

        foreach (var entry in ChangeTracker.Entries<Base>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    if (_loggedInUserService != null)
                        entry.Entity.UserId = _loggedInUserService.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountBalance>().HasKey(ab => ab.Id);
        modelBuilder.Entity<AccountBalance>().Property(ab => ab.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Bet>().HasKey(b => b.Id);
        modelBuilder.Entity<Bet>().Property(b => b.Id).ValueGeneratedOnAdd();


    }
}
