using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Omega.Core.Models;
using Omega.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUser(string id);
        Task<IEnumerable<UsersTableDto>> GetUsers();
        Task<IdentityResult> RegisterUser(UserRegisterDto model);
        Task<IdentityResult> UpdateUser(string id, UserEditDto newUser);
        Task<IdentityResult> DeleteUser(string id);
        Task<bool> UserExists(string username, string id = null);
        Task<bool> EmailExists(string email, string id = null);

    }
    public class UsersRepository : IUsersRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UsersRepository(MyDbContext context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<IEnumerable<UsersTableDto>> GetUsers()
        {
            var usersTable = new List<UsersTableDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userForTable = _mapper.Map<UsersTableDto>(user);
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Where(r => r == "Admin").Any())
                    userForTable.isAdmin = true;
                else
                    userForTable.isAdmin = false;

                usersTable.Add(userForTable);
            }
            return usersTable;
        }
        public async Task<User> GetUser(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<IdentityResult> UpdateUser(string id, UserEditDto newUser)
        {
            var oldUser = await _context.Users.FindAsync(id);
            oldUser.UserName = newUser.UserName;
            oldUser.Email = newUser.Email;
            var result = await _userManager.UpdateAsync(oldUser);

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                if (newUser.IsAdmin)
                    await _userManager.AddToRoleAsync(oldUser, UserRoles.Admin);
                else
                    await _userManager.RemoveFromRoleAsync(oldUser, UserRoles.Admin);
            }

            return result;
        }
        public async Task<IdentityResult> RegisterUser(UserRegisterDto model)
        {
            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.IsAdmin)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }
                }
            }
            return result;
        }
        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var userRoles = await _context.UserRoles.Where(r => r.UserId == user.Id).ToListAsync();
            foreach (var role in userRoles)
                _context.UserRoles.Remove(role);
                _context.SaveChanges();

            var result = await _userManager.DeleteAsync(user);
            return result;
        }
        public async Task<bool> UserExists(string username, string id = null)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (user != null)
                        return true;
                }
                else
                {
                    if (user.Id != id)
                        return true;
                }
            }
            return false;
        }
        public async Task<bool> EmailExists(string email,string id = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (user != null)
                        return true;
                }
                else
                {
                    if (user.Id != id)
                        return true;
                }
            }
            return false;
        }
    }
}
