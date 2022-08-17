using System.ComponentModel.DataAnnotations;

namespace ikeamind_backend.Core.Models.EFModels.AccountModels
{
    public partial class Account
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenCreated { get; set; }
        public string RefreshTokenExpires { get; set; }
    }
}
