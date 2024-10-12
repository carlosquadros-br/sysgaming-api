using System;
using SysgamingApi.Src.Application.Bets.Dtos;

namespace SysgamingApi.Src.Application.Bets.Command;

public static class CreateBetValidator
{
    public static void Validate(CreateBetRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        AmountIsBiggerThanZero(request);
        AmountIsBiggerThanMinimum(request);
    }

    private static void AmountIsBiggerThanZero(CreateBetRequest request)
    {
        var passed = request.Amount > 0;
        if (!passed)
        {
            throw new ArgumentException("Amount must be greater than 0");
        }
    }

    private static void AmountIsBiggerThanMinimum(CreateBetRequest request)
    {
        var passed = request.Amount > 1;
        if (!passed)
        {
            throw new ArgumentException("Amount must be greater than 1");
        }
    }
}
