using System.Data;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Domain.Persitence.Repositories;

public interface IBetRepository : IBaseRepository<Bet>
{

    Task<Bet> UpdateAsync(Bet bet);
}