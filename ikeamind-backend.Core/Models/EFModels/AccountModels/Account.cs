using System;

namespace ikeamind_backend.Core.Models.EFModels.AccountModels
{
    public partial class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenCreated { get; set; }
        public string RefreshTokenExpires { get; set; }
    }
}
