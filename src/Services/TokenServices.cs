using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.WebApi.Models;
using AspNetCore.WebApi;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AspNetCore.WebApi.Extensions;
using System.Security.Cryptography;

namespace AspNetCore.WebApi.Services
{
    public static class TokenServices
    {

        public static string GenerateToken(User user)
        {

            var TokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.Secret);
            var claims = user.GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };

            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token);

        }

       
        
    }
}
