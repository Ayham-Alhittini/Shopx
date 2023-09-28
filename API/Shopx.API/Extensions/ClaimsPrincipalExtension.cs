using System.Security.Claims;

namespace Shopx.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }
        public static string GetAccountState(this ClaimsPrincipal user)
        {
            return user.FindFirst("AccountState")?.Value;
        }
        public static string GetAccountType(this ClaimsPrincipal user)
        {
            return user.FindFirst("AccountType")?.Value;
        }


    }
}
