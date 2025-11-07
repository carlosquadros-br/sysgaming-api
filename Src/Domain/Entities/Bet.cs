using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Enums;

namespace SysgamingApi.Src.Domain.Entities;


[Table("Bet"), Serializable]
public class Bet : Base
{
    [JsonIgnore]
    public User? User { get; set; }
    public decimal Amount { get; set; }
    public BetResult Result { get; set; } = BetResult.None;
    public BetStatus Status { get; set; } = BetStatus.ACTIVE;

    public DateTime FinishAt { get; set; }

    public DateTime CanceledAt { get; set; }
}
