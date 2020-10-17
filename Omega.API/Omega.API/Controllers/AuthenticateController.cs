using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omega.Infrastructure.Dtos;
using Omega.Infrastructure.Repositories;

namespace Omega.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthenticateController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var token = await _repo.Login(user);
            if (token == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "نام یا رمز عبور وارد شده صحیح نیست" });


            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
