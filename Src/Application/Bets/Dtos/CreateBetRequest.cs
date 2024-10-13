using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SysgamingApi.Src.Application.Bets.Dtos;

public class CreateBetRequest
{

    [Required]
    public decimal Amount { get; set; }

}
