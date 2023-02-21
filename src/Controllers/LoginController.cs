using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AspNetCore.WebApi.Data;
using AspNetCore.WebApi.Models;
using AspNetCore.WebApi.Services;

namespace AspNetCore.WebApi.Controllers
{
    [ApiController]
    public class LoginController:ControllerBase
    {

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] Context context,
            [FromBody] User model)
            
        {

            if(!ModelState.IsValid)
            return BadRequest();

                var user = new User{
                Id = model.Id,
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            };

 
            var token = TokenServices.GenerateToken(user);
            var refreshToken = RefreshTokenServices.GenerateRefreshToken();
            RefreshTokenServices.SaveRefreshToken(user.Username, refreshToken);
            
           try
           {

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();
            user.Password = " ";

            return new
            {

                user = user,
                token = token,
                refreshToken = refreshToken
            };

           }

           catch(Exception e){

            return e.ToString();
           }
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(string token,string refreshToken)
        {
          
            var principal = RefreshTokenServices.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = RefreshTokenServices.GetRefreshToken(username);
            if(savedRefreshToken != refreshToken)
            throw new SecurityTokenException("Invalid Refresh");

            var newJwtToken = RefreshTokenServices.GenerateNewToken(principal.Claims);
            var newRefreshToken = RefreshTokenServices.GenerateRefreshToken();
            RefreshTokenServices.DeleteRefreshToken(username, refreshToken);
            RefreshTokenServices.SaveRefreshToken(username, newRefreshToken);

           return new ObjectResult(new {
            
            token = newJwtToken,
            refreshToken = newRefreshToken
           });


        }
        
        
        
    }
}

