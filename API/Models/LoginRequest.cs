using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class LoginRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}