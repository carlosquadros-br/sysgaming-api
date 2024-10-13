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

    public override DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>();
    }

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

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

        //Validators:V-1
        modelBuilder.Entity<User>(entity =>
        {

            entity.ToTable("User");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email).IsRequired();
            entity.HasAlternateKey(u => u.Email);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<AccountBalance>().HasKey(ab => ab.Id);
        modelBuilder.Entity<AccountBalance>().Property(ab => ab.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<AccountBalance>().HasOne(ab => ab.User).WithOne(u => u.AccountBalance).HasForeignKey<AccountBalance>(ab => ab.UserId);

        modelBuilder.Entity<Bet>().HasKey(b => b.Id);
        modelBuilder.Entity<Bet>().Property(b => b.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();
            entity.HasOne(t => t.User).WithMany(u => u.Transactions).HasForeignKey(t => t.UserId);
        });
    }
}
