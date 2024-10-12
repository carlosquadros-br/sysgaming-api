using System;

namespace SysgamingApi.Src.Domain.Entities.BetState;

public class BaseBet : IBetState
{

    public void Create()
    {
        //  se a tive bet ja for criada entao nao vai fazer isso
        System.Console.WriteLine("Not create");
    }

    public virtual void Active()
    {
        // nao vai faze isso
        System.Console.WriteLine("Not Active");
    }
}
