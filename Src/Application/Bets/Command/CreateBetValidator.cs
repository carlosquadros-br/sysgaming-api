using System;
using FluentValidation;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Bets.Command;

public class CreateBetValidator : AbstractValidator<CreateBetRequest>
{

    public CreateBetValidator() : base()
    {
        RuleFor(x => x.Amount).NotEmpty();
        AmountIsBiggerThanZero();
        AmountIsBiggerThanMinimum();
    }

    private void AmountIsBiggerThanZero()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be bigger than 0");
    }

    private void AmountIsBiggerThanMinimum()
    {
        RuleFor(x => x.Amount).GreaterThan(1).WithMessage("Amount must be bigger than 1");
    }
}
