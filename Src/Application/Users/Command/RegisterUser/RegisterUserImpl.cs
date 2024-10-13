using System;
using SysgamingApi.Src.Application.AccountBalances.Command.CreateAccountBalance;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Application.Users.Command.LoginUser;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Users.Command.RegisterUser;

public class RegisterUserImpl : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginUserUseCase _loginUserUseCase;

    private readonly RegisterUserValidator _validator;
    private readonly ICreateAccountBalanceUseCase _createAccountBalanceUseCase;

    public RegisterUserImpl(
        IUserRepository userRepository,
        ILoginUserUseCase loginUserUseCase,
        ICreateAccountBalanceUseCase createAccountBalanceUseCase
        )
    {
        _userRepository = userRepository;
        _loginUserUseCase = loginUserUseCase;
        _validator = new RegisterUserValidator(userRepository);
        _createAccountBalanceUseCase = createAccountBalanceUseCase;
    }

    public async Task<TokenResponse> HandleAsync(RegisterRequest request)
    {

        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new Exception(validationResult.ToString());

        try
        {
            await _userRepository.GetbyEmail(request.Email);
        }
        catch (Exception)
        {
            // Validators: V-1
            throw new Exception("Email User already exists");
        }


        var user = new User { UserName = request.Name, Email = request.Email };
        var createdUser = await _userRepository.CreateAsync(user, request.Password) ?? throw new Exception("User not created");

        await _createAccountBalanceUseCase.Handle(createdUser.Id, 0);


        return await _loginUserUseCase.HandleAsync(new LoginRequest { Email = request.Email, Password = request.Password });
    }
}
