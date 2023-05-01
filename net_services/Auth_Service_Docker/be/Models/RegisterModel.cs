using System.ComponentModel.DataAnnotations;

namespace be.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserType { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
