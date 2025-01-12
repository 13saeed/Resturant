using Microsoft.AspNetCore.Identity;

namespace Resturant.Models
{
    public class Users : IdentityUser
    {
        public string? FullName { get; set; }

    }
}
