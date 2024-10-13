using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysgamingApi.Src.Domain.Entities;

public abstract class Base : IBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [ForeignKey("UserId")]
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
