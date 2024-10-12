using System;
using Microsoft.AspNetCore.Identity;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class UserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
}
