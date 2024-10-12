using System;

namespace SysgamingApi.Src.Application.Dtos;

public record TokenResponse(
    string Token,
    DateTime Expires
)
{ }
