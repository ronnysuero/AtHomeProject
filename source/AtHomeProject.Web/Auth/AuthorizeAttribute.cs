using System;
using System.Collections.Generic;
using System.Linq;
using AtHomeProject.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtHomeProject.Web.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeAttribute(string role) => _role = role;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var unauthorized = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Unauthorized"
            };

            if (context.HttpContext.Items["User"] is not Users x)
                context.Result = new JsonResult(unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };

            if (
                context.HttpContext.Items["UserClaims"] is IEnumerable<UsersClaims> claims &&
                !claims.Any(c => c.ClaimType == Constants.ROLE && c.ClaimValue == _role)
            )
            {
                context.Result = new JsonResult(unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
