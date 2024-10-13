namespace SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;

public record OperationResponse(
    bool Sucess,
    string Type,
    string Description,
    decimal Amount,
    decimal CurrentBalanceUser,
    string UserId,
    string UserName,
    string Currency,
    DateTime UpadatedAt
)
{
}
