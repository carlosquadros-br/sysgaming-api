using System;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Domain.Persitence.Repositories;

public interface IUserRepository
{

    Task<User> CreateAsync(User entity, string password);

    Task<User> LoginAsync(string email, string password);

    Task<User> GetbyEmail(string email);
    Task<User> GetByIdAsync(string userId);
}
