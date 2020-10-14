using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Omega.Core.Models;
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
                return Unauthorized();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            var userExists = await _repo.UserExists(model.UserName);
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین نام در سیستم ثبت شده." });

            var emailExists = await _repo.EmailExists(model.Email);
            if (emailExists)
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین ایمیل در سیستم ثبت شده." });

            var result = await _repo.Register(model);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "ثبت کاربر با مشکل مواجه شد لطفا ورودی های خود را چک کرده و مجددا تلاش کنید." });

            return Ok(new { Status = "Success", Message = "کاربر با موفقیت ثبت شد." });
        }
    }
}
