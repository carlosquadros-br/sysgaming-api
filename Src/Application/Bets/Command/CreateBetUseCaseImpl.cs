using System;
using AutoMapper;
using FluentValidation;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Bets.Command;

public class CreateBetUseCaseImpl : ICreateBetUseCase
{
    private readonly IBetRepository _betRepository;
    private readonly AbstractValidator<CreateBetRequest> _createBetValidator;
    private readonly IMapper _mapper;

    public CreateBetUseCaseImpl(
        IBetRepository betRepository,
        IMapper mapper)
    {
        _betRepository = betRepository;
        _createBetValidator = new CreateBetValidator();
        _mapper = mapper;
    }

    public async Task<Bet> ExecuteAsync(CreateBetRequest request)
    {
        var isValid = await _createBetValidator.ValidateAsync(request);

        var response = new BaseResponse();
        if (isValid.Errors.Count > 0)
        {
            response.Success = false;
            response.ValidationErrors = new List<string>();
            foreach (var error in isValid.Errors)
            {
                response.ValidationErrors.Add(error.ErrorMessage);
            }
        }

        if (!response.Success)
            return await Task.FromResult<Bet>(null);


        return await Task.FromResult(
            await _betRepository.CreateAsync(_mapper.Map<Bet>(request))
            );

    }

}
