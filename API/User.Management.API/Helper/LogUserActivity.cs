using Microsoft.AspNetCore.Mvc.Filters;
using Shopx.API.Data;
using Shopx.API.Extensions;
using Shopx.API.Interfaces;

namespace Shopx.API.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            if (userId == null) return;
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(userId);

            if (user != null)
            {
                user.LastActive = DateTime.UtcNow;
                await repo.SaveAllAsync();
            }
        }
    }
}
