using System;
using SysgamingApi.Src.Application.Dtos;

namespace SysgamingApi.Src.Application.Users.Command.LoginUser;

public class LoginUseCaseImpl : ILoginUserUseCase
{
    public Task<TokenResponse> HandleAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}
