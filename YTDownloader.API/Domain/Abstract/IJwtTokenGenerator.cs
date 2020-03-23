using System;
using System.IdentityModel.Tokens.Jwt;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(int userId, string username, string tokenKey, DateTime tokenExpiration);
    }
}
