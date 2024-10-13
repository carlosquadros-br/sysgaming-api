namespace SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;

public record DepositResponse(
    decimal Balance,
    string UserId,
    string UserName,
    string Currency,
    DateTime UpadatedAt
)
{
}
