using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
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
        private readonly IUserRepository _userRepository;

        public AccountBalanceController(
            IDepositAccountUseCase depositAccountUseCase,
            IUserRepository userRepository)
        {
            _depositAccountUseCase = depositAccountUseCase;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("get-my-balance")]
        public IActionResult GetMyBalance()
        {
            return Ok("R$ 100,00");
        }

        [Authorize]
        [HttpPost]
        [Route("deposit")]
        public async Task<ActionResult<DepositResponse>> Deposit(decimal amount)
        {
            return await _depositAccountUseCase.Handle(amount);

        }

    }
}
