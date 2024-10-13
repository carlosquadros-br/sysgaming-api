using System;
using SysgamingApi.Src.Domain.Entities;
namespace SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;

public interface ICreateTransactionUseCase
{
    Task<Transaction> Handle(CreateTransactionRequest request);
}
