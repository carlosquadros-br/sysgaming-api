using System;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Users.Command.RegisterUser;

public class RegisterUserImpl : IRegisterUserUseCase
{
    private readonly object _userRepository;

    public RegisterUserImpl()
    {
        _userRepository = new object();
    }

    public Task<TokenResponse> HandleAsync(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}
