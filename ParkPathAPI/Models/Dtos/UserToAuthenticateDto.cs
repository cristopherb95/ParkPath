using System.ComponentModel.DataAnnotations;

namespace ParkPathAPI.Models
{
    public class UserToAuthenticateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}