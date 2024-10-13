using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

// IBetState.cs
public interface IBetState
{
    Task<UpdateBetDto> ChangeStateAsync(Bet bet);
}
