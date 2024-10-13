using System;
using System.Text.Json;
using AutoMapper;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;
public class CreateTransactionUseCaseImpl : ICreateTransactionUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly INotifcationService _notificationService;

    public CreateTransactionUseCaseImpl(
        ITransactionRepository transactionRepository,
        IMapper mapper,
        INotifcationService notifcationService
        )
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _notificationService = notifcationService;
    }

    public async Task<Transaction> Handle(CreateTransactionRequest request)
    {
        try
        {
            var transaction = _mapper.Map<Transaction>(request);
            var result = await _transactionRepository.CreateAsync(transaction);
            // add websocket
            SendMessage(transaction);
            return result;
        }
        catch (Exception e)
        {
            throw new System.Exception("Error creating transaction");
        }
    }

    private void SendMessage(Transaction transaction)
    {
        _notificationService.SendNotificationAsync(JsonSerializer.Serialize(transaction));
    }
}
