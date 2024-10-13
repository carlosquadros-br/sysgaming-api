using System;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Dtos;

public record TokenResponse(
    string Token,
    DateTime Expires,
    User User
)
{ }
