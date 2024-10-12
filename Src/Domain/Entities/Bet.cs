using System;
using System.ComponentModel.DataAnnotations.Schema;
using SysgamingApi.Src.Domain.Entities.BetState;

namespace SysgamingApi.Src.Domain.Entities;


[Table("Bets")]
public class Bet : Base
{
    public string? UserId { get; set; }

    public string? GameId { get; set; }

    public string? TeamId { get; set; }

    public decimal Amount { get; set; }

    public IBetState? Status { get; }
}
