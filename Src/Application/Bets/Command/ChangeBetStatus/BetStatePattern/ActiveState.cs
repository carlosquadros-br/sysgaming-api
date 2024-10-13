using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus.BetStatePattern;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

// ActiveState.cs
public class ActiveState : AbstractBetSate, IBetState
{
    public ActiveState(IBetRepository betRepository) : base(betRepository)
    {
    }
}
