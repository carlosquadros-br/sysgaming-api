using System;
using SysgamingApi.Src.Application.Transactions.Processor;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain;
using SysgamingApi.Src.Domain.Enums;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Domain.Struct;

namespace SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;

public class DepositAccountUseCaseImpl : IDepositAccountUseCase
{

    private readonly ILoggedInUserService _loggedInUserService;
    private readonly IAccountBalanceRepository _accountBalanceRepository;
    private readonly IUserRepository _userRepository;

    public DepositAccountUseCaseImpl(
        ILoggedInUserService loggedInUserService,
        IAccountBalanceRepository accountBalanceRepository,
        IUserRepository userRepository)
    {
        _loggedInUserService = loggedInUserService;
        _accountBalanceRepository = accountBalanceRepository;
        _userRepository = userRepository;
    }

    [Transaction]
    public async Task<OperationResponse> Handle(decimal request)
    {
        var userId = _loggedInUserService.UserId;

        if (userId == null)
            throw new Exception("User not logged in");


        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");


        if (request <= 0)
            throw new Exception("Invalid amount to deposit : " + request);


        try
        {
            var accountBalance = await _accountBalanceRepository.UpdateBalanceAsync(userId, request);
            var currency = "";

            if (accountBalance.Currency == Domain.Enums.Currency.BRL)
            {
                currency = "BRL";
            }

            var response = new OperationResponse(
                true,
                TransactionType.INPUT,
                TransactionDescription.DEPOSIT,
                request,
                accountBalance.Balance,
                userId,
                user.UserName,
                currency,
                accountBalance.UpdatedAt
            );
            return response;
        }
        catch (Exception e)
        {
            return new OperationResponse(
                false,
                TransactionType.INPUT,
                TransactionDescription.DEPOSIT,
                request,
                0,
                userId,
                user.UserName,
                Currency.BRL.ToString(),
                DateTime.Now
            );

        };


    }
}
