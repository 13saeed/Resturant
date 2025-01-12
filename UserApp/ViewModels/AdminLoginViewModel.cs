using System.ComponentModel.DataAnnotations;

namespace Resturant.ViewModels
{
    public class AdminLoginViewModel
    {
            [Required(ErrorMessage = "UserName is required.")]
            public string? Username { get; set; }
            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            public string? Password { get; set; }
    }
}
