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

    public DateTime FinishAt { get; set; }

    public DateTime CanceledAt { get; set; }

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
        // Define o status da aposta como finalizado
        Status = BetStatus.FINISHED;
        FinishAt = DateTime.UtcNow;
        // Gera um resultado aleatório para a aposta (ganhou ou perdeu)
        var random = new Random();
        bool isWin = random.Next(2) == 0; // Gera 0 ou 1 de forma aleatória

        // Atualiza o resultado com base no valor aleatório
        Result = isWin ? BetResult.Win : BetResult.Lose;


        return true;
    }

    public bool CancelBet()
    {
        if (Status != BetStatus.ACTIVE)
        {
            return false;
        }
        Status = BetStatus.CANCELED;
        CanceledAt = DateTime.UtcNow;
        return true;
    }
}
