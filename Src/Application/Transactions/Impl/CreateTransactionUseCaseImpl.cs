using System;
using System.Transactions;

namespace SysgamingApi.Src.Application.Transactions.Impl;

public class CreateTransactionUseCaseImpl : ICreateTransactionUseCase
{
    public Task<Transaction> Handle(CreateTransactionRequest request)
    {
        throw new NotImplementedException();
    }
}
