using System;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Application.Transactions.Processor;
using SysgamingApi.Src.Domain;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Enums;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Domain.Struct;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

public class ChangeBetSatusUseCaseImpl : IChangeBetStatusUseCase
{
    private readonly IBetRepository _betRepository;
    private readonly IAccountBalanceRepository _accountBalanceRepository;

    public ChangeBetSatusUseCaseImpl(IBetRepository betRepository,
        IAccountBalanceRepository accountBalanceRepository
    )
    {
        _betRepository = betRepository;
        _accountBalanceRepository = accountBalanceRepository;
    }


    [Transaction]
    public async Task<OperationResponse> Handle(string betId, BetStatus status)
    {
        var bet = await _betRepository.GetByIdAsync(betId);

        if (bet == null)
        {
            return new OperationResponse(false, null, "Bet not found!", 0, 0, null, null, null, DateTime.UtcNow);
        }

        var oldStatus = bet.Status;
        System.Console.WriteLine($"Changing bet status from {oldStatus} to {status}");
        IBetState newState = status switch
        {
            BetStatus.ACTIVE => new ActiveState(_betRepository),
            BetStatus.FINISHED => new FinishedState(_betRepository),
            BetStatus.CANCELED => new CanceledState(_betRepository),
            _ => throw new ArgumentOutOfRangeException()
        };
        var change = await newState.ChangeStateAsync(bet);

        if (!change.updated)
        {
            return new OperationResponse(false, null, change.message, 0, 0, null, null, null, DateTime.UtcNow);
        }

        var saved = await _betRepository.UpdateAsync(bet);
        if (saved != null)
        {
            return await HandleWithAccountBalance(bet);
        }
        return new OperationResponse(false, null, "Error saving bet", 0, 0, null, null, null, DateTime.UtcNow);


    }

    private async Task<OperationResponse> HandleWithAccountBalance(Bet bet)
    {
        if (bet == null || bet.Status == BetStatus.ACTIVE)
        {
            return new OperationResponse(false, null, null, 0, 0, null, null, null, DateTime.UtcNow);
        }

        var accountBalance = await _accountBalanceRepository.GetByUserIdAsync(bet.UserId);

        if (accountBalance == null)
        {
            return new OperationResponse(false, null, null, 0, 0, null, null, null, DateTime.UtcNow);
        }
        var value = bet.Amount;
        var result = bet.Status switch
        {
            BetStatus.FINISHED => await HandleFinished(bet, accountBalance),
            BetStatus.CANCELED => await HandleCanceled(bet, accountBalance),
            _ => throw new ArgumentOutOfRangeException()
        };
        return result;

    }

    private async Task<OperationResponse> HandleFinished(Bet bet, AccountBalance accountBalance)
    {
        if (bet.Result == BetResult.None)
        {
            return new OperationResponse(false, null, null, 0, 0, null, null, null, DateTime.UtcNow);
        }
        var value = bet.Amount;

        if (bet.Result == BetResult.Lose)
        {
            return new OperationResponse(true, TransactionType.OUTPUT, $"{TransactionDescription.BET} : LOST ", value, accountBalance.Balance, bet.UserId, bet?.User?.UserName ?? " ", Currency.BRL.ToString(), DateTime.UtcNow);
        }

        value = bet.Amount * 2;
        if (await _betRepository.IsLastFiveBetsIsLose(bet.UserId))
        {
            var totalLastFiveBets = await _betRepository.GetAmountLastFiveBets(bet.UserId);
            value = bet.Amount + (totalLastFiveBets * 0.1m);
        }

        var teste = await _accountBalanceRepository.UpdateBalanceAsync(bet.UserId, bet.Amount * 2);
        return new OperationResponse(true, TransactionType.OUTPUT, $"{TransactionDescription.BET} : WON ", value, accountBalance.Balance, bet.UserId, bet?.User?.UserName ?? " ", Currency.BRL.ToString(), DateTime.UtcNow);
    }


    private async Task<OperationResponse> HandleCanceled(Bet bet, AccountBalance accountBalance)
    {
        if (bet.Result != BetResult.None)
            return new OperationResponse(false, null, null, 0, 0, null, null, null, DateTime.UtcNow);
        var value = bet.Amount;
        var AccountUpdated = await _accountBalanceRepository.UpdateBalanceAsync(bet.UserId, bet.Amount);
        return new OperationResponse(true, TransactionType.OUTPUT, $"{TransactionDescription.BET} : CANCELED ", value, accountBalance.Balance, bet.UserId, bet?.User?.UserName ?? " ", Currency.BRL.ToString(), DateTime.UtcNow);
    }



}