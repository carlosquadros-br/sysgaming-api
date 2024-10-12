using System;

namespace SysgamingApi.Src.Application.Bets.Dtos;

public class CreateBetRequest
{
    public string UserId { get; set; }

    public string GameId { get; set; }

    public string TeamId { get; set; }

    public decimal Amount { get; set; }

}
