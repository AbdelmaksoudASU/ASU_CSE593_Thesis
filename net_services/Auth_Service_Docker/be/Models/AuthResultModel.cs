using System.ComponentModel.DataAnnotations;

namespace be.Models
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
