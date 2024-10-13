using System;
using System.Text;
using AutoMapper;
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
using SysgamingApi.Src.Application.Transactions;
using SysgamingApi.Src.Application.Transactions.Impl;
using SysgamingApi.Src.Application.Users.Command.LoginUser;
using SysgamingApi.Src.Application.Users.Command.RegisterUser;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Infrastructure.Persistence;
using SysgamingApi.Src.Presentation.Services;

namespace SysgamingApi.Src.Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
    {
        AddMapper(services);
        AddValidators(services);

        //balance-account
        services.AddScoped<ICreateAccountBalanceUseCase, CreateAccountBalanceUseCaseImpl>();
        services.AddScoped<IDepositAccountUseCase, DepositAccountUseCaseImpl>();

        // bet
        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCaseImpl>();
        services.AddScoped<ICreateBetUseCase, CreateBetUseCaseImpl>();
        services.AddScoped<IChangeBetStatusUseCase, ChangeBetStatusUseCaseImpl>();

        // user
        services.AddScoped<ILoginUserUseCase, LoginUseCaseImpl>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserImpl>();


        services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppPostgresDbContext>()
        .AddDefaultTokenProviders();

        System.Console.WriteLine("Adding Application DI");

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
                        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated");
                        return Task.CompletedTask;
                    }
                };
            });


        return services;

    }

    private static void AddMapper(IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(typeof(BetMapper));
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
