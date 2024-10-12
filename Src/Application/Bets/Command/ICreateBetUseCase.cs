using System;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Bets.Command;

public interface ICreateBetUseCase
{
    Task<Bet> ExecuteAsync(CreateBetRequest request);
}
