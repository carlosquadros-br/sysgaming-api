using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Application.Bets.Command;
using SysgamingApi.Src.Application.Bets.Dtos;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        readonly IBetRepository _repository;
        readonly ICreateBetUseCase _createBetUseCase;
        public BetController(
            IBetRepository repository,
            ICreateBetUseCase createBetUseCase
            )
        {
            _repository = repository;
            _createBetUseCase = createBetUseCase;
        }

        [Route("find-paginated")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationList<Bet>>> GetBetsPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _repository.GetPaginatedAsync(pageNumber, pageSize);
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Bet>> CreateBet([FromBody] CreateBetRequest request)
        {
            var bet = await _createBetUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(CreateBet), bet);
        }


    }
}
