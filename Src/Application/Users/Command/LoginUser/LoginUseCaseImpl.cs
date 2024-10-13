using System;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Application.Users.Token;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Users.Command.LoginUser;

public class LoginUseCaseImpl : ILoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;

    public LoginUseCaseImpl(IUserRepository userRepository, ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
    }

    public async Task<TokenResponse> HandleAsync(LoginRequest request)
    {
        var user = await _userRepository.LoginAsync(request.Email, request.Password) ?? throw new Exception("User not found");

        var expirationTime = DateTime.UtcNow.AddMinutes(60);

        System.Console.WriteLine("User logged in");
        System.Console.WriteLine("User: " + user.UserName);
        System.Console.WriteLine("Expiration time: " + expirationTime);
        try
        {
            var token = _tokenProvider.GenerateToken(user);

        }
        catch (System.Exception e)
        {
            System.Console.WriteLine("Error generating token: " + e.Message);
            throw;
        }

        return new TokenResponse(_tokenProvider.GenerateToken(user), expirationTime, user);
    }
}
