using System;
using SysgamingApi.Src.Domain.Entities.BetState;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

public interface IChangeBetStatusUseCase
{

    public Task<bool> Handle(string betId, BetStatus status);

}
