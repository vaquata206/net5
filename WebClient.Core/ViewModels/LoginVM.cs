using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
