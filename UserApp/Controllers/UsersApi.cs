using Microsoft.AspNetCore.Mvc;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersApi(UserServices userServices) : ControllerBase
    {
        private readonly UserServices _userServices = userServices;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _userServices.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<Users>> GetUserById(string userId)
        {
            var user = await _userServices.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(string userId, Users user)
        {
            if (userId != user.Id)
            {
                return BadRequest();
            }
            await _userServices.UpdateUserAsync(user);
            return NoContent();

        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            await _userServices.DeleteUserAsync(userId);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            var user = new Users
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email
            };

            await _userServices.CreateUserAsync(user, model.Password, model.Role);
            return Ok();
        }

        public class CreateUserModel
        {
            public string? FullName { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }
        }

    }
}
