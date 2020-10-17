using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omega.Core.Models;
using Omega.Infrastructure.Dtos;
using Omega.Infrastructure.Repositories;

namespace Omega.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private IUsersRepository _repo;

        public UsersController(IUsersRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<object> GetAllUsers()
        {
            var response = await _repo.GetUsers();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterDto model)
        {
            var userExists = await _repo.UserExists(model.UserName);
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین نام در سیستم ثبت شده" });

            var emailExists = await _repo.EmailExists(model.Email);
            if (emailExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین ایمیل در سیستم ثبت شده" });

            var result = await _repo.RegisterUser(model);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "ثبت کاربر با مشکل مواجه شد لطفا ورودی های خود را چک کرده و مجددا تلاش کنید" });

            return Ok(new { Status = "Success", Message = "کاربر با موفقیت ثبت شد." });
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser(string id,UserEditDto model)
        {
            var userExists = await _repo.UserExists(model.UserName,id);
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین نام در سیستم ثبت شده" });

            var emailExists = await _repo.EmailExists(model.Email,id);
            if (emailExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "کاربر دیگری با همین ایمیل در سیستم ثبت شده" });

            var result = await _repo.UpdateUser(id,model);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "ثبت کاربر با مشکل مواجه شد لطفا ورودی های خود را چک کرده و مجددا تلاش کنید" });

            return Ok(new { Status = "Success", Message = "کاربر با موفقیت بروز رسانی شد." });
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _repo.DeleteUser(id);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "حذف کاربر با مشکل مواجه شد" });

            return Ok(new { Status = "Success", Message = "کاربر با موفقیت حذف شد." });
        }
    }
}
