using System.ComponentModel.DataAnnotations;

namespace be.Models
{
    public class TokenRequestModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
