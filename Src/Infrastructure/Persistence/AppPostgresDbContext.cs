using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;

namespace SysgamingApi.Src.Infrastructure.Persistence;

public class AppPostgresDbContext : IdentityDbContext<User>, IAppDbContext
{

    public virtual DbSet<Bet> Bets { get; set; }

    public AppPostgresDbContext(DbContextOptions<AppPostgresDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bet>().HasKey(b => b.Id);
        modelBuilder.Entity<Bet>().Property(b => b.Id).ValueGeneratedOnAdd();


    }
}
