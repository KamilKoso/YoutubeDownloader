using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YTDownloader.API.Domain.Abstract;

namespace YTDownloader.API.Domain.Entities
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(int userId, string username, string tokenKey, DateTime tokenExpiration)
        {
            if (tokenExpiration < DateTime.Now)
                throw new ArgumentException("Token expiration date is less than server date", "tokenExpiration");

            var claims = GenerateClaims(userId, username);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = GenerateDescriptor(claims, tokenExpiration, creds);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
             
            return tokenHandler.WriteToken(token);
        }

        private Claim[] GenerateClaims(int userId, string username)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
            };
            return claims;
        }

        private SecurityTokenDescriptor GenerateDescriptor(Claim[] claims, DateTime tokenExpiration, SigningCredentials signingCredentials)
        {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiration,
                SigningCredentials = signingCredentials,
            };

            return tokenDescriptor;
        }
    }
}
