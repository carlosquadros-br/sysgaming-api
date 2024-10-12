using System;
using AutoMapper;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.Bets.Mapper
{
    public class BetMapper : Profile
    {
        public BetMapper() : base()
        {
            CreateMap<CreateBetRequest, Bet>().ReverseMap();
        }
    }
}
