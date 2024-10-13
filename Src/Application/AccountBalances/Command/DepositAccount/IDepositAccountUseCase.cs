using System;

namespace SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;

public interface IDepositAccountUseCase
{

    Task<OperationResponse> Handle(decimal request);
}
