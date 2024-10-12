using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Entities;
namespace SysgamingApi.Src.Domain.Persitence;

public interface IAppDbContext : IDisposable
{

    DbSet<Bet> Bets { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
