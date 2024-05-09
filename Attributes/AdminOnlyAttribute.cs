
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using HouseMates.Areas.Identity.Data;
using System;

namespace HouseMates.Attributes
{
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Get services from DI container
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var applicationUser = userManager.GetUserAsync(user).Result;

            // Check if the current user has admin privileges
            if (applicationUser == null || !applicationUser.IsAdmin)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
