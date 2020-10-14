using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Omega.Core.Models;
using Omega.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> Register(UserRegisterDto model);
        Task<JwtSecurityToken> Login(UserLoginDto model);
        Task<bool> UserExists(string username);
        Task<bool> EmailExists(string email);
    }
    public class AuthRepsitory : IAuthRepository
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepsitory(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<JwtSecurityToken> Login(UserLoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            // in case the input is the user's email address
            if (user == null)
                user = await userManager.FindByEmailAsync(model.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return token;
            }
            return null;
        }

        public async Task<IdentityResult> Register(UserRegisterDto model)
        {
            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.IsAdmin)
                {
                    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    if (!await roleManager.RoleExistsAsync(UserRoles.User))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                    if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }
                }
            }
            return result;
        }
        public async Task<bool> UserExists(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
                return true;
            return false;
        }
        public async Task<bool> EmailExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return true;
            return false;
        }
    }
}
