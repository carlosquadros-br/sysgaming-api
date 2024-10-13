using System;
using AutoMapper;
using SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Transactions.Mapper;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<CreateTransactionRequest, Transaction>().ReverseMap();
    }
}
