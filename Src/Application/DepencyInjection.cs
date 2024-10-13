using System;
using System.Text;
using AutoMapper;
using Castle.DynamicProxy;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SysgamingApi.Src.Application.AccountBalances.Command.CreateAccountBalance;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
using SysgamingApi.Src.Application.Bets.Command;
using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Application.Bets.Mapper;
using SysgamingApi.Src.Application.Common.Queries.GetListPaginated;
using SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;
using SysgamingApi.Src.Application.Users.Command.LoginUser;
using SysgamingApi.Src.Application.Users.Command.RegisterUser;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Infrastructure.Persistence;
using SysgamingApi.Src.Application.Transactions.Processor;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Application.Transactions.Mapper;

namespace SysgamingApi.Src.Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGetDataPaginateUseCase<>), typeof(GetDataPaginateUseCaseImpl<>));

        AddMapper(services);
        AddValidators(services);


        //transaction
        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCaseImpl>();

        // Register the interceptor
        services.AddScoped<IInterceptor, TransactionInterceptor>();
        var proxyGenerator = new ProxyGenerator();

        //balance-account
        services.AddScoped<ICreateAccountBalanceUseCase, CreateAccountBalanceUseCaseImpl>();
        services.AddScoped<DepositAccountUseCaseImpl>();
        services.AddScoped(provider =>
        {
            var interceptor = provider.GetRequiredService<IInterceptor>();
            var target = provider.GetRequiredService<DepositAccountUseCaseImpl>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<IDepositAccountUseCase>(target, interceptor);
        });

        services.AddScoped<CreateBetUseCaseImpl>();

        // bet
        services.AddScoped<ICreateBetUseCase>(provider =>
        {
            var interceptor = provider.GetRequiredService<IInterceptor>();
            var target = provider.GetRequiredService<CreateBetUseCaseImpl>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<ICreateBetUseCase>(target, interceptor);
        });

        services.AddScoped<ChangeBetSatusUseCaseImpl>();
        services.AddScoped<IChangeBetStatusUseCase>(provider =>
        {
            var interceptor = provider.GetRequiredService<IInterceptor>();
            var target = provider.GetRequiredService<ChangeBetSatusUseCaseImpl>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<IChangeBetStatusUseCase>(target, interceptor);
        });

        // user
        services.AddScoped<ILoginUserUseCase, LoginUseCaseImpl>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserImpl>();

        services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppPostgresDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })

            // 4. Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddSingleton<INotifcationService, NotificationServiceImpl>();
        return services;
    }


    private static void AddMapper(IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfiles(typeof(BetMapper),
            typeof(TransactionMapper));
        });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateBetValidator>());

        services.AddScoped<IValidator<CreateBetRequest>, CreateBetValidator>();
    }
}
