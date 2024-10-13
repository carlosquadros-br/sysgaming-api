using System;
using System.ComponentModel.DataAnnotations.Schema;
using SysgamingApi.Src.Domain.Enums;

namespace SysgamingApi.Src.Domain.Entities;

[Table("AccountBalance"), Serializable]
public sealed class AccountBalance : Base
{
    public Currency Currency { get; set; } = Currency.BRL;

    [ForeignKey("UserId")]
    public User User { get; set; }

    public string UserId { get; set; }

    public decimal Balance { get; set; }

    public override string ToString()
    {
        return @$"AccountBalance: {{
        Id: {Id},
        Currency: {Currency},
        User: {User},
        UserId: {UserId},
        Balance: {Balance},
        CreatedAt: {CreatedAt},
        UpdatedAt: {UpdatedAt}
    }}";
    }
}
