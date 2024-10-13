using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus.BetStatePattern;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

// CanceledState.cs
public class CanceledState : AbstractBetSate, IBetState
{
    public CanceledState(IBetRepository betRepository) : base(betRepository)
    {
    }

    override public async Task<UpdateBetDto> ChangeStateAsync(Bet bet)
    {
        var canceled = bet.CancelBet();
        if (!canceled)
            return new UpdateBetDto(false, "Não foi possível cancelar a aposta");
        return new UpdateBetDto(true, "Aposta cancelada com sucesso");
    }
}