using System;
using SysgamingApi.Src.Domain.Enums;

namespace SysgamingApi.Src.Domain.Entities;

public sealed class AccountBalance : Base
{
    Currency Currency { get; set; } = Currency.BRL;

    decimal Balance { get; }

    private void UpdateBalance()
    {
        this.Balance.ToString();
    }
}
