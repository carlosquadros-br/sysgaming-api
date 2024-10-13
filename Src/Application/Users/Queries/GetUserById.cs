using System;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Users.Queries;

public interface IGetUserByIdUseCase
{

    public Task<User> Handle(string userId);

}
