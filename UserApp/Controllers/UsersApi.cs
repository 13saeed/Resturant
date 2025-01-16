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

    }
}
