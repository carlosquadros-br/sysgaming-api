using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus.BetStatePattern;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

// FinishedState.cs
public class FinishedState : AbstractBetSate, IBetState
{
    public FinishedState(IBetRepository betRepository) : base(betRepository)
    {
    }

    override public async Task<UpdateBetDto> ChangeStateAsync(Bet bet)
    {
        var finished = bet.FinishBet();
        if (!finished)
        {
            var text = "Não foi possível finalizar a aposta";
            if (bet.Status == BetStatus.CANCELED)
            {
                text = "Não é possível finalizar uma aposta cancelada";
            }
            return await Task.FromResult(new UpdateBetDto(false, text));
        }
        return await Task.FromResult(new UpdateBetDto(true, "Aposta finalizada com sucesso"));
    }
}
