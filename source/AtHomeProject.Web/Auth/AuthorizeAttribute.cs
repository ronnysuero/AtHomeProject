using System;
using AtHomeProject.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtHomeProject.Web.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Role _role;

        public AuthorizeAttribute(Role role) => _role = role;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var unauthorized = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Unauthorized"
            };

            switch (_role)
            {
                case Role.Device:
                {
                    if (context.HttpContext.Items["User"] is not DeviceModel)
                        context.Result = new JsonResult(unauthorized)
                            { StatusCode = StatusCodes.Status401Unauthorized };

                    break;
                }
                case Role.User:
                {
                    if (context.HttpContext.Items["User"] is not UserModel)
                        context.Result = new JsonResult(unauthorized)
                            { StatusCode = StatusCodes.Status401Unauthorized };

                    break;
                }
                default:
                    context.Result = new JsonResult(unauthorized)
                        { StatusCode = StatusCodes.Status401Unauthorized };

                    break;
            }
        }
    }
}
