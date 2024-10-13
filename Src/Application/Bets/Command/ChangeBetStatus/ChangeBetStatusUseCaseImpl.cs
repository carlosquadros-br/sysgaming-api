using System;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

public class ChangeBetStatusUseCaseImpl : IChangeBetStatusUseCase
{
    private readonly IBetRepository _betRepository;

    public ChangeBetStatusUseCaseImpl(IBetRepository betRepository)
    {
        _betRepository = betRepository;
    }

    public async Task<bool> Handle(string betId, BetStatus status)
    {
        var bet = await _betRepository.GetByIdAsync(betId);
        if (bet == null)
        {
            throw new Exception("Bet not found");
        }
        IBetState newState = status switch
        {
            BetStatus.ACTIVE => new ActiveState(),
            BetStatus.FINISHED => new FinishedState(),
            BetStatus.CANCELED => new CanceledState(),
            _ => throw new ArgumentOutOfRangeException()
        };
        return newState.ChangeState(bet);
    }
}


// IBetState.cs
public interface IBetState
{
    bool ChangeState(Bet bet);
}

// ActiveState.cs
public class ActiveState : IBetState
{
    public bool ChangeState(Bet bet)
    {
        return false;
    }
}

// FinishedState.cs
public class FinishedState : IBetState
{
    public bool ChangeState(Bet bet)
    {
        return bet.FinishBet();
    }
}

// CanceledState.cs
public class CanceledState : IBetState
{
    public bool ChangeState(Bet bet)
    {
        return bet.CancelBet();
    }
}