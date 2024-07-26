using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}