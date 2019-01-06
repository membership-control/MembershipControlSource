using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Identity.Models;
using Core.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace WebMembership
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute  
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var vr = new ViewResult();
            // filterContext.Result = vr;
            filterContext.HttpContext.Response.StatusCode = 403;

            if (filterContext.HttpContext.Request.Form.AllKeys.Length == 8 )
            {
             
                Core.Infrastructure.Dev.DevResponse a = new Core.Infrastructure.Dev.DevResponse();
                a.haveError = true;
                a.error = "Missing";
                a.totalCount = 0;
                filterContext.Result = new WebMembership.MVC.NewJsonResult(a);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                       {
                           { "action", "SessionTimeout" },
                           { "controller", "Account" },
                           { "Area",  ""}
                       });
            }
            //filterContext.Result = new HttpUnauthorizedResult();
//            routes.MapRoute(
//"CreateAdditionalPreviousNames", // Route name
//"Users/{controller}/{action}/{userId}/{applicantId}", // URL with parameters
//new { controller = "UsersAdditionalPreviousNames", action = "Index" });

//            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Manage", action = "Index" }));
            
//            filterContext.Result = new RedirectResult("~/manage/index");
        } 
    }
}