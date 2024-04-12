using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class SuperAdminFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var authenticatedUser = context.HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;

        // Проверяем, является ли пользователь администратором
        if (authenticatedUser != null && !string.Equals(authenticatedUser.UserRole, "SuperAdmin", StringComparison.InvariantCultureIgnoreCase))
        {
            context.Result = new StatusCodeResult(403); // 403 Forbidden
            return;
        }
    }
}