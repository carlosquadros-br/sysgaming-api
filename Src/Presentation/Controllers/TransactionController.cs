using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        readonly ITransactionRepository _repository;

        TransactionController(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [Route("find-paginated")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationList<Transaction>>> GetBetsPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _repository.GetPaginatedAsync(pageNumber, pageSize);
        }


    }
}
