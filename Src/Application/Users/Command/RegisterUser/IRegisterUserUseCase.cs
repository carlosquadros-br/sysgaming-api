using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Users.Command.RegisterUser;

public interface IRegisterUserUseCase
{
    Task<TokenResponse> HandleAsync(RegisterRequest request);
}
