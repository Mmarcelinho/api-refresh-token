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
    public static class RefreshTokenServices
    {
         public static string GenerateNewToken(IEnumerable<Claim> claims)
        {

            var TokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.Secret);
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

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var TokenValidationParameters = 
            new TokenValidationParameters
        {

            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.Secret)),
            ValidateLifetime = false
            
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out var securityToken);

        if(securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals( 
            SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase) )
        throw new SecurityTokenException(" Invalid Token ");

        
        return principal;

        }

        private static List<(string,string)> _refreshTokens = new();

        public static void SaveRefreshToken( string username, string refreshToken)
        {
            _refreshTokens.Add(new(username, refreshToken));
        }

        public static string GetRefreshToken( string username)
        {
            return _refreshTokens.FirstOrDefault(x => x.Item1 == username).Item2;
        }

        public static void DeleteRefreshToken( string username, string refreshToken)
        {
            var item = _refreshTokens.FirstOrDefault(x => x.Item1 == username && x.Item2 == refreshToken);
            _refreshTokens.Remove(item);
        }

    }
}