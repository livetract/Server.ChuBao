using Api.Server.ChuBao.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<IdentityUser> userManager,
            //SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            this._userManager = userManager;
            //this._signInManager = signInManager;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<Results<Accepted, BadRequest>> Register(UserDto userDto)
        {
            _logger.LogInformation($"正在验证邮箱");
            if (!ModelState.IsValid)
            {
                return TypedResults.BadRequest();
            }
            try
            {
                var user = _mapper.Map<IdentityUser>(userDto);
                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded) 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        _logger.LogError($"{nameof(Register)}-- {ModelState.Values}");
                    }
                    return TypedResults.BadRequest();
                }

                await _userManager.AddToRolesAsync(user, userDto.Roles);

                var location = Url.Action(nameof(ContactController.GetContacts));
                return TypedResults.Accepted(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注册的时候出现了错误：{nameof(Register)}");
                return TypedResults.BadRequest();
            }
        }

        [HttpPost]
        public async Task<Results<Accepted, UnauthorizedHttpResult,BadRequest<string>>> Login(UserDto userDto)
        {
            _logger.LogInformation($"正在验证邮箱");
            if (!ModelState.IsValid)
            {
                return TypedResults.BadRequest($"{ModelState}模型验证失败。");
            }
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password,false,false);
                if (!result.Succeeded)
                {
                    return TypedResults.Unauthorized();
                    //return TypedResults.BadRequest($"用户登录失败。");
                }
                return TypedResults.Accepted(Url.Action(nameof(Login),userDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"登录的时候出现了错误：{nameof(Register)}");
                return TypedResults.BadRequest($"登录的时候出现了错误：{nameof(Register)}");
            }
        }

    }
}
