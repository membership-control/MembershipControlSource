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
using System.Web.Mvc;



namespace WebMembership.CustomFilters
{
    public class AuthLogAttribute : AuthorizeAttribute
    {
        public AuthLogAttribute()
        {
            View = "AccessDenied";
        }

        public string View { get; set; }
        
        public PermissionLevel PermissionLevel { get; set; }
        public string permission_msg { get; set; }
        public string Area { get; set; }

        private bool _customGridAuth = false;
        public bool CustomGridAuth
        {
            get { return _customGridAuth; }
            set { _customGridAuth = value; }
        }
       


        /// <summary>
        /// Check for Authorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
          
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var vr = new ViewResult();
            vr.ViewName = View;
           // filterContext.Result = vr;
            filterContext.HttpContext.Response.StatusCode = 403;



            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                       {
                           { "action", "SessionTimeout" },
                           { "controller", "Account" },
                           { "Area",  ""}
                       });
            //filterContext.Result = new HttpUnauthorizedResult();
            //            routes.MapRoute(
            //"CreateAdditionalPreviousNames", // Route name
            //"Users/{controller}/{action}/{userId}/{applicantId}", // URL with parameters
            //new { controller = "UsersAdditionalPreviousNames", action = "Index" });



            //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Manage", action = "Index" }));

            //            filterContext.Result = new RedirectResult("~/manage/index");
        } 
 

        /// <summary>
        /// Method to check if the user is Authorized or not
        /// if yes continue to perform the action else redirect to error page
        /// </summary>
        /// <param name="filterContext"></param>
        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                return;
            }
            var username = filterContext.HttpContext.User.Identity.Name;
            var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var user = userManager.FindByName(username);
            bool isAuthorize = false;

            if (user == null)
            {                
//                throw new Exception("User not found!");

            }
            else
            {
                var userRoleIds = (from r in user.Roles select r.RoleId);

                var userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r).ToList();

                // Skip Authorization process if found Admin role attached to user
                if (userRoles.Any(r => r.Name == "Admin"))
                    return;

                // If using DevDatagrid, use auth below
                if (CustomGridAuth)
                {
                    string actionValue = filterContext.HttpContext.Request.Form.AllKeys.Contains("action") ? filterContext.HttpContext.Request.Form.GetValues("action").FirstOrDefault() : "";

                    switch (actionValue)
                    {
                        case "create":
                            this.PermissionLevel = Core.Identity.Models.PermissionLevel.Create;
                            break;

                        case "update":
                            this.PermissionLevel = Core.Identity.Models.PermissionLevel.Update;
                            break;

                        case "delete":
                            this.PermissionLevel = Core.Identity.Models.PermissionLevel.Delete;
                            break;

                        default:
                            return;
                    }
                }

                // Check by [Roles:] tag attribute first
                if (!string.IsNullOrEmpty(this.Roles))
                {
                    var role_params = this.Roles.Split(',');
                    var userRoleNames = userRoles.Select(ur => ur.Name).ToList();

                    isAuthorize = userRoleNames.Any(ur => Array.IndexOf(role_params, ur) > -1);
                }
                // Check by user level; then check by role level if no permission found on user level
                else
                {
                    bool isUserPermissionAreaFound = false;
                    isUserPermissionAreaFound = user.UserPermissions.Any(p => p.PermissionModel.Id == this.Area);

                    isAuthorize = user.UserPermissions.Any(p => p.PermissionModel.Id == this.Area && HasPermission(p.Level));
                    // Same permission area will take user's level
                    if (!isAuthorize && !isUserPermissionAreaFound)
                    {
                        isAuthorize = userRoles.Any(z => z.RolePermissions.Any(p => p.PermissionModel.Id == this.Area && HasPermission(p.Level)));
                    }
                }
                // If the Result returns null (default filter) AND custom isAuthorize above true then the user is Authorized 
                if (filterContext.Result == null && isAuthorize)
                    return;

                bool isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
                //If the user is Un-Authorized then Navigate to Auth Failed View 
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    var vr = new ViewResult();
                    vr.ViewName = View;
                    vr.ViewBag.PermissionMessage = this.Area + "  " + this.PermissionLevel.ToString();
                    var result = vr;

                    if (!isAjaxRequest)
                    {
                        //var vr = new ViewResult();
                        //vr.ViewName = View;
                        //var result = vr;
                        filterContext.RouteData.DataTokens["area"] = "";
                        filterContext.Result = result;
                    }
                    else
                    {
                        //                    filterContext.HttpContext.Response.StatusCode = 403;

                        //filterContext.HttpContext.Response.End();
                        //filterContext.RouteData.DataTokens["area"] = "";
                        //filterContext.Result = result; //new RedirectResult("~/Manage/" + this.View);
                        if (this.PermissionLevel.ToString().ToUpper() != "READ")
                        {
                            if (this.PermissionLevel.ToString().ToUpper() == "ALL")
                            {
                                filterContext.Result = new WebMembership.MVC.NewJsonResult(false);
                            }
                            else
                            {
                                permission_msg = vr.ViewBag.PermissionMessage;
                                Core.Infrastructure.Dev.DevResponse a = new Core.Infrastructure.Dev.DevResponse();
                                a.haveError = true;
                                a.error = "Missing " + this.PermissionLevel.ToString() + " permission for " + this.Area;
                                filterContext.Result = new WebMembership.MVC.NewJsonResult(a);
                            }
                            //filterContext.Result = new JsonResult()
                            //{
                            //    Data = new
                            //    {
                            //        ErrorMessage = vr.ViewBag.PermissionMessage,
                            //        RedirectUrl = _customGridAuth ? filterContext.HttpContext.Request.ApplicationPath + "/Manage/" + this.View + "?errmsg=" + vr.ViewBag.PermissionMessage : filterContext.HttpContext.Request.RawUrl + "?errmsg=" + vr.ViewBag.PermissionMessage

                            //    },
                            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            //};
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                           {
                               { "action", "View1" },
                               { "controller", "Manage" },
                               { "Area", ""},
                                 { "errmsg",  vr.ViewBag.PermissionMessage }
                           });
                        }
                    }
                }
            }
        }

        private bool HasPermission(int permissionLevel)
        {
            return ((Core.Identity.Models.PermissionLevel)permissionLevel & this.PermissionLevel) == this.PermissionLevel;
        }
    }
}