using System;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Users.Token;

public interface ITokenProvider
{
    public string GenerateToken(User user);
}
