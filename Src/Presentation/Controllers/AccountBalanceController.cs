using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
using SysgamingApi.Src.Application.AccountBalances.Queries;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using System.Linq;
using System.Security.Claims;

namespace SysgamingApi.Src.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountBalanceController : ControllerBase
    {
        private readonly IDepositAccountUseCase _depositAccountUseCase;
        private readonly IGetAccountBalanceByUserId _getAccountBalanceByuserId;

        public AccountBalanceController(
            IDepositAccountUseCase depositAccountUseCase,
            IGetAccountBalanceByUserId getAccountBalanceByUserId
            )
        {
            _depositAccountUseCase = depositAccountUseCase;
            _getAccountBalanceByuserId = getAccountBalanceByUserId;
        }

        [Authorize]
        [HttpGet]
        [Route("get-my-balance")]
        public async Task<ActionResult<AccountBalance>> GetMyBalance()
        {
            return await _getAccountBalanceByuserId.HandleAsync();
        }

        [Authorize]
        [HttpPost]
        [Route("deposit")]
        public async Task<ActionResult<OperationResponse>> Deposit(decimal amount)
        {
            return await _depositAccountUseCase.Handle(amount);
        }

    }
}
