using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Application.Common.Queries.GetListPaginated;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        private readonly IGetDataPaginateUseCase<Transaction> _getTransactionPaginatedUseCase;

        public TransactionController(
            IGetDataPaginateUseCase<Transaction> getTransactionPaginatedUseCase)
        {
            _getTransactionPaginatedUseCase = getTransactionPaginatedUseCase;
        }

        [Route("find-paginated")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationList<Transaction>>> GetBetsPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _getTransactionPaginatedUseCase.Handle(pageNumber, pageSize);
        }


    }
}
