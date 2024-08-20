using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security.Claims;

namespace Shoopi.Helper
{
    public class ShoopiAuthorizedAddtribute : TypeFilterAttribute
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }
        public ShoopiAuthorizedAddtribute(string roleName, string actionValue) : base(typeof(tuanAuthorizeFilter))
        {
            RoleName = roleName;
            ActionValue = actionValue;
            Arguments = new object[] { roleName, actionValue };
        }
    }
    public class tuanAuthorizeFilter : IAuthorizationFilter
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }

        public tuanAuthorizeFilter(string roleName, string actionValue)
        {
            RoleName = roleName;
            ActionValue = actionValue;

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated || !CanAccessToAction(context.HttpContext))
            {
                context.Result = new ForbidResult();
            }
        }


        private bool CanAccessToAction(HttpContext httpContext)
        {
            var roles = httpContext.User.FindFirstValue(ClaimTypes.Role);
            if (roles != null && roles.Equals(RoleName))
            {
                return true;
            }
            return false;
        }

    }
}

