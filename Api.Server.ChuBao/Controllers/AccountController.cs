using AutoMapper;
using Core.Server.ChuBao.DTOs;
using Data.Server.Chubao.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<IdentityUser> userManager,
            IAuthManager authManager,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            this._userManager = userManager;
            this._authManager = authManager;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            _logger.LogInformation($"正在验证邮箱");
            if (!ModelState.IsValid)
            {
                return BadRequest("It's a error model");
            }

            var user = _mapper.Map<IdentityUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    _logger.LogError($"{nameof(Register)}-- {ModelState.Values}");
                }
                return BadRequest();
            }

            await _userManager.AddToRolesAsync(user, model.Roles);

            return Accepted();

        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            _logger.LogInformation($"正在验证邮箱");
            if (!ModelState.IsValid)
            {
                return BadRequest($"{ModelState}模型验证失败。");
            }

            if (!await _authManager.ValidateUser(userDto))
            {
                return Unauthorized();
            }
            var token = await _authManager.CreateToken();
            return Ok(token);
        }

    }
}
