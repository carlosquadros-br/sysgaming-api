using System;
using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus.BetStatePattern;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Enums;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;

// FinishedState.cs
public class FinishedState : AbstractBetSate, IBetState
{
    private static readonly Random _random = new Random();
    
    public FinishedState(IBetRepository betRepository) : base(betRepository)
    {
    }

    override public async Task<UpdateBetDto> ChangeStateAsync(Bet bet)
    {
        if (bet.Status != BetStatus.ACTIVE)
        {
            var text = "Não foi possível finalizar a aposta";
            if (bet.Status == BetStatus.CANCELED)
            {
                text = "Não é possível finalizar uma aposta cancelada";
            }
            return await Task.FromResult(new UpdateBetDto(false, text));
        }
        
        // Define o status da aposta como finalizado
        bet.Status = BetStatus.FINISHED;
        bet.FinishAt = DateTime.UtcNow;
        
        // Gera um resultado aleatório para a aposta (ganhou ou perdeu)
        bool isWin = _random.Next(2) == 0; // Gera 0 ou 1 de forma aleatória

        // Atualiza o resultado com base no valor aleatório
        bet.Result = isWin ? BetResult.Win : BetResult.Lose;
        
        return await Task.FromResult(new UpdateBetDto(true, "Aposta finalizada com sucesso"));
    }
}
