namespace SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;

public record CreateTransactionRequest(
    string? Description,
    string? TransactionType,
    decimal? Amount,
    decimal CurrentBalanceUser,
    string UserName,
    string Currency
);
