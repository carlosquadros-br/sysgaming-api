using System;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities.BetState;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

public interface IChangeBetStatusUseCase
{

    public Task<OperationResponse> Handle(string betId, BetStatus status);

}
