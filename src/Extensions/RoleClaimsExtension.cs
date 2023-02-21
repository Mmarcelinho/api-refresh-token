using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.WebApi.Models;

namespace AspNetCore.WebApi.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new ( ClaimTypes.Name, user.Username),
                new ( ClaimTypes.Role, user.Role)
            };

            return result;
        
        }
        
    }
}