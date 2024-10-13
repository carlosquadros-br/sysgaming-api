using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Application.Bets.Command;
using SysgamingApi.Src.Application.Bets.Command.ChangeBetStatus;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Entities.BetState;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        readonly IBetRepository _repository;
        readonly ICreateBetUseCase _createBetUseCase;
        private readonly IChangeBetStatusUseCase _changeBetStatusUseCase;
        private readonly ILoggedInUserService _loggedInUserService;

        public BetController(
            IBetRepository repository,
            ICreateBetUseCase createBetUseCase,
            IChangeBetStatusUseCase changeBetStatusUseCase,
            ILoggedInUserService loggedInUserService
            )
        {
            _repository = repository;
            _createBetUseCase = createBetUseCase;
            _changeBetStatusUseCase = changeBetStatusUseCase;
            _loggedInUserService = loggedInUserService;
        }

        [Authorize]
        [Route("find-paginated")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginationList<Bet>>> GetBetsPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            System.Console.WriteLine("Getting bets paginated");
            return await _repository.GetPaginatedAsync(pageNumber, pageSize);
        }

        [Authorize]
        [Route("create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Bet>> CreateBet([FromBody] CreateBetRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Execute use case to create bet
                var bet = await _createBetUseCase.ExecuteAsync(request);
                if (bet == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Bet creation failed. Please try again later.");
                }

                return CreatedAtAction(nameof(CreateBet), bet);
            }
            catch (ValidationException ex)
            {
                System.Console.WriteLine($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message, details = ex.ValidationResult });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [Route("change-status/{betId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeBetStatus([FromRoute] string betId, [FromRoute] BetStatus status)
        {
            var result = await _changeBetStatusUseCase.Handle(betId, status);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }


    }
}
