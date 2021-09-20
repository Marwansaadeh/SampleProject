using System.ComponentModel.DataAnnotations;

namespace MobilApp.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public int CustomerId { get; set; }
    }
}
