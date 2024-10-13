using System;

namespace SysgamingApi.Src.Domain.Entities;

public interface IBase
{

    string Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }

}
