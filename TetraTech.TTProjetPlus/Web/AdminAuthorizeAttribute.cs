using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Extensions;

namespace TetraTech.TTProjetPlus.Web
{
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.IsAdmin())
                filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}