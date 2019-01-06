using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Core.Data.Model;
using Core.Identity;

namespace WebMembership.Controllers
{
    public class BaseController : Controller
    {

        public string UserName
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }

        public string UserID
        {
            get
            {
                //System.Security.Claims.ClaimsIdentity ci = HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
                //try
                //{
                //    string id = ci.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                //    return id;
                //}
                //catch
                //{
                //    return null;
                //}
                return HttpContext.User.Identity.GetUserId();
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public LogModel Log
        {
            get
            {
                return new LogModel()
                {
                    Insert_User = User.Identity.Name,
                    Insert_Date = DateTime.Now,
                    Client = Request.UserHostAddress,
                    SessionID = Session.SessionID,
                    Mode_ID = String.Concat(this.ControllerContext.RouteData.Values["controller"].ToString(), '/',
                            this.ControllerContext.RouteData.Values["action"].ToString()),
                    UserID = UserID
                };
            }
        }

        public string Role
        {
            get
            {
                System.Security.Claims.ClaimsIdentity ci = HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
                string id = ci.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role").Value;
                return id;
            }
        }

    }
}