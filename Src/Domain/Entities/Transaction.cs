using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SysgamingApi.Src.Domain.Entities;

public class Transaction : Base
{
    [JsonIgnore]
    [NotMapped]
    public User User { get; set; }
    public string? Description { get; set; }
    public string? TransactionType { get; set; }
    public decimal Amount { get; set; }
    public decimal CurrentBalanceUser { get; set; }
    public string? UserName { get; set; }
    public string? Currency { get; set; }
}
