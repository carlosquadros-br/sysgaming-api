using System;

namespace SysgamingApi.Src.Domain.Entities;

public class Player : Base
{

    private void GetId()
    {
        this.Id = Guid.NewGuid();
    }
}
