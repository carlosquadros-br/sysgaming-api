using System;
using AutoMapper;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;
public class CreateTransactionUseCaseImpl : ICreateTransactionUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public CreateTransactionUseCaseImpl(
        ITransactionRepository transactionRepository,
        IMapper mapper
        )
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<Transaction> Handle(CreateTransactionRequest request)
    {
        try
        {
            var transaction = _mapper.Map<Transaction>(request);
            var result = await _transactionRepository.CreateAsync(transaction);
            // add websocket
            System.Console.WriteLine("Transaction created");
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            throw new System.Exception("Error creating transaction");
        }
    }
}
