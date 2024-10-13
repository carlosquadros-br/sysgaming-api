using System;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Users.Queries;

public class GetUserByIdUseCaseImpl : IGetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdUseCaseImpl(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(string userId)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var user = await _userRepository.GetByIdAsync(userId);
        return user;
    }
}
