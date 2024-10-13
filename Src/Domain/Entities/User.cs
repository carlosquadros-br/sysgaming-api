using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SysgamingApi.Src.Domain.Entities;

//Validators:V-1
[Table("User"), Serializable, Index(nameof(Email), IsUnique = true)]
public class User : IdentityUser, IBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string Id { get; set; }

    [JsonIgnore]
    public AccountBalance? AccountBalance { get; set; }

    [JsonIgnore]
    public List<Bet> Bets { get; set; } = new List<Bet>();

    [JsonIgnore]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
