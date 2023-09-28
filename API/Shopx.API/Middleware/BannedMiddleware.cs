using Microsoft.AspNetCore.Identity;
using Shopx.API.Data;
using Shopx.API.Entities;
using Shopx.API.Errors;
using Shopx.API.Extensions;
using Shopx.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Shopx.API.Middleware
{
    public class BannedMiddleware
    {
        private readonly RequestDelegate _next;

        public BannedMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                // Decode the token claims
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                // Access the claims from the token
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;

                if (userId != null)
                {
                    var user = await userRepository.GetUserByIdAsync(userId);

                    if (user.AccountState == States.banned)
                    {
                        throw new ArgumentException("Your banned by the admin currently");
                    }

                }
            }


            await _next(context);
        }
    }
}
