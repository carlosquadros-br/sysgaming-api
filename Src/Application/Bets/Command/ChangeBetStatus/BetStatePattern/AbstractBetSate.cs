using System;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus.BetStatePattern;

public abstract class AbstractBetSate : IBetState
{
    protected readonly IBetRepository _betRepository;

    public AbstractBetSate(
        IBetRepository betRepository
    )
    {
        _betRepository = betRepository;
    }

    public virtual async Task<UpdateBetDto> ChangeStateAsync(Bet bet)
    {
        return await Task.Run(() =>
        {
            return new UpdateBetDto(false, "Não foi possível alterar o estado da aposta");
        });
    }
}
