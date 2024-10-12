using System;
using SysgamingApi.Src.Application.Dtos;

namespace SysgamingApi.Src.Application.Users.Command.LoginUser;

public interface ILoginUserUseCase
{
    Task<TokenResponse> HandleAsync(LoginRequest request);

}
