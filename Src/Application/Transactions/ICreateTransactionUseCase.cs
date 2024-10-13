using System;
using System.Transactions;

namespace SysgamingApi.Src.Application.Transactions;

public interface ICreateTransactionUseCase
{
    Task<Transaction> Handle(CreateTransactionRequest request);

}

public class CreateTransactionRequest
{

}