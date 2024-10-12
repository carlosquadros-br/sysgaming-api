using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysgamingApi.Src.Domain.Entities;

public abstract class Base
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

}
