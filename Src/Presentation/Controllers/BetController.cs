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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Execute use case to create bet
            var bet = await _createBetUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(CreateBet), bet);
        }

        [Route("cancel/{betId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CancelBet([FromRoute] string betId)
        {
            var result = await _changeBetStatusUseCase.Handle(betId, BetStatus.CANCELED);
            if (result.Sucess)
            {
                return Ok(new
                {
                    message = "Bet canceled successfully",
                    data = new
                    {
                        status = result.Sucess,
                        message = result.Description
                    }
                });
            }
            return StatusCode(StatusCodes.Status406NotAcceptable,
            new { message = "Error canceling bet", data = result });
        }

        [Route("finish/{betId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> FinishBet([FromRoute] string betId)
        {
            var result = await _changeBetStatusUseCase.Handle(betId, BetStatus.FINISHED);
            if (result.Sucess)
            {
                return Ok(new
                {
                    success = true,
                    message = "Bet finished successfully",
                    data = new
                    {
                        status = result.Sucess,
                        message = result.Description
                    }
                });
            }
            return StatusCode(StatusCodes.Status406NotAcceptable,
            new { success = false, message = "Error finishing bet", data = result });
        }
    }
}
