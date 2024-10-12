using System;
using SysgamingApi.Src.Application.Bets.Command;
using SysgamingApi.Src.Application.Transactions;
using SysgamingApi.Src.Application.Transactions.Impl;

namespace SysgamingApi.Src.Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCaseImpl>();
        services.AddScoped<ICreateBetUseCase, CreateBetUseCaseImpl>();
        return services;
    }
}
