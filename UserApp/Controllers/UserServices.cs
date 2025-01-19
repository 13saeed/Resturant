using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Resturant.Models;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using Resturant.Data;

namespace Resturant.Controllers
{
    public class UserServices
    {
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context;

        public UserServices(UserManager<Users> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }
        public async Task<Users?> GetUserByIdAsync(string userid)
        {
            if (userid == null)
            {
                return null;
            }
            return await _userManager.FindByIdAsync(userid);
        }
        public async Task UpdateUserAsync(Users user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.UserName = user.UserName;
                await _userManager.UpdateAsync(existingUser);
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task CreateUserAsync(Users user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception("Failed to create user");
            }
        }
    }
}
