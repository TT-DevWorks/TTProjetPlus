using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus
{
    public class CustomErrorHandler : HandleErrorAttribute
    {   
        public override void OnException(ExceptionContext filterContext)
        {          
            CustomErrorHandlerService _customErrorHandlerService = new CustomErrorHandlerService();
            string strInnerException = filterContext.Exception.InnerException != null ? filterContext.Exception.InnerException.ToString() : filterContext.Exception.Message;
            _customErrorHandlerService.InsertError(filterContext.HttpContext.User.Identity.Name, UserService.CurrentUser.TTAccount.ToString(), filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString(), strInnerException, filterContext.HttpContext.Request.UrlReferrer.OriginalString);           
        }
    }
} 