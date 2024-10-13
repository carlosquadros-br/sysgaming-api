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
    public decimal Amount { get; private set; }
    public BetResult Result { get; set; } = BetResult.None;
    public BetStatus Status { get; private set; } = BetStatus.ACTIVE;

    public string GetStatusName()
    {
        return Status switch
        {
            BetStatus.ACTIVE => "ATIVO",
            BetStatus.FINISHED => "FINALIZADO",
            BetStatus.CANCELED => "CANCELADO",
            _ => "INDEFINIDO",
        };
    }

    public bool FinishBet()
    {
        if (Status != BetStatus.ACTIVE)
        {
            return false;
        }
        Status = BetStatus.FINISHED;
        return true;
    }

    public bool CancelBet()
    {
        System.Console.WriteLine("STATOS  ATUAL" + Status);
        if (Status != BetStatus.ACTIVE)
        {
            return false;
        }
        Status = BetStatus.CANCELED;
        return true;
    }
}
