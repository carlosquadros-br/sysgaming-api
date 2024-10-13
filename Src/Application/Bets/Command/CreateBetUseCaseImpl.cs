using System;
using AutoMapper;
using FluentValidation;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Enums;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command;

public class CreateBetUseCaseImpl : ICreateBetUseCase
{
    private readonly IBetRepository _betRepository;
    private readonly IAccountBalanceRepository _accountBalanceRepository;
    private readonly AbstractValidator<CreateBetRequest> _createBetValidator;

    private readonly ILoggedInUserService _loggedInUserService;
    private readonly IMapper _mapper;

    public CreateBetUseCaseImpl(
        IBetRepository betRepository,
        IAccountBalanceRepository accountBalanceRepository,
        ILoggedInUserService loggedInUserService,
        IMapper mapper)
    {
        _betRepository = betRepository;
        _accountBalanceRepository = accountBalanceRepository;
        _loggedInUserService = loggedInUserService;
        _createBetValidator = new CreateBetValidator();
        _mapper = mapper;
    }

    public async Task<Bet> ExecuteAsync(CreateBetRequest request)
    {
        var isValid = await _createBetValidator.ValidateAsync(request);

        var response = new BaseResponse();
        if (isValid.Errors.Count > 0)
        {
            response.Success = false;
            response.ValidationErrors = new List<string>();
            foreach (var error in isValid.Errors)
            {
                response.ValidationErrors.Add(error.ErrorMessage);
            }
        }

        if (!response.Success)
            return await Task.FromResult<Bet>(null);

        var bet = _mapper.Map<Bet>(request);

        bet.UserId = _loggedInUserService.UserId;

        if (bet.UserId == null)
        {
            throw new Exception("User not logged in");
        }

        if (!await UserHaveEnoughBalance(bet))
        {
            throw new Exception("User does not have enough balance");
        }


        bet.Result = BetResult.None;


        bet = await _betRepository.CreateAsync(bet);

        if (bet == null)
        {
            throw new Exception("Bet not created");
        }

        if (!await UpdateUserBalance(bet))
        {
            throw new Exception("Error updating user balance");
        }

        return bet;

    }

    private async Task<bool> UserHaveEnoughBalance(Bet bet)
    {

        if (bet == null || bet.UserId == null)
        {
            return await Task.FromResult(false);
        }

        var account = await _accountBalanceRepository.GetByUserIdAsync(bet.UserId);

        return account != null && account.Balance >= bet.Amount;
    }

    private async Task<bool> UpdateUserBalance(Bet bet)
    {
        if (bet == null || bet.UserId == null)
        {
            return await Task.FromResult(false);
        }
        var account = await _accountBalanceRepository.UpdateBalanceAsync(bet.UserId, -bet.Amount);
        System.Console.WriteLine("Account balance updated: " + account.Balance);
        return account != null;
    }

}
