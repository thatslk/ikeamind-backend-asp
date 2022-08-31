using System;

namespace ikeamind_backend.Core.Models.ReturnModels
{
    public record TokensRM
    {
        public string AccessToken { get; set; }
        public RefreshTokenDTO RefreshTokenData { get; set; }
    }

    public record RefreshTokenDTO
    {
        public string RefreshToken { get; set; }
        public string ExpiresDate { get; set; }
    }
}
