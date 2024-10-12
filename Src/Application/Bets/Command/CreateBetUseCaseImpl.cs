using System;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Bets.Command;

public class CreateBetUseCaseImpl : ICreateBetUseCase
{

    public Task<Bet> ExecuteAsync(CreateBetRequest request)
    {
        CreateBetValidator.Validate(request);
        var bet = new Bet
        {
            UserId = request.UserId,
            GameId = request.GameId,
            TeamId = request.TeamId,
            Amount = request.Amount
        };

        return Task.FromResult(bet);
    }
}
