using System;

namespace SysgamingApi.Src.Application.Bets.Dtos;

public record UpdateBetDto(
    bool updated,
    string message
)
{
}
