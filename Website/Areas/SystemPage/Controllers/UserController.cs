using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebMembership.Controllers;
using WebMembership.CustomFilters;
using WebMembership.Areas.SystemPage.Models;
using WebMembership.MVC;
using Core.Identity.Models;
using Core.Services.Interface;
using Newtonsoft.Json;
using Core.Services.DTO.SystemPage;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System.Threading.Tasks;
using Core.Identity;
using Core.Identity.Models;

namespace WebMembership.Areas.SystemPage.Controllers
{
    [CustomAuthorize]
    public class UserController : BaseController
    {
        private Core.Services.Interface.IUser _IUser;
        private ApplicationUserManager _userManager;
        
        public UserController(Core.Services.Interface.IUser iIUser,ApplicationUserManager userManager)
        {
            this._IUser = iIUser;
            _userManager = userManager;
        }
        public ActionResult DevPageData()
        {
            var results = this._IUser.GetAllUsers(HttpContext.Request.Form, this.Log);
            System.Guid id;
            if (System.Guid.TryParse(results.key, out id))
            {
               var result =  _userManager.RemovePassword(id.ToString());
               result = _userManager.AddPassword(id.ToString(), "welcome1");
            }
            return new WebMembership.MVC.NewJsonResult(results);
        }
        public ActionResult ResetPassword(string id)
        {   
            try
            {
                var result = _userManager.RemovePassword(id.ToString());
                result = _userManager.AddPassword(id.ToString(), "welcome1");
                return new WebMembership.MVC.NewJsonResult(true);
            }
            catch (Exception ex)
            { return new WebMembership.MVC.NewJsonResult(false); }
        }
        public ActionResult RoleLookup()
        {
            return new WebMembership.MVC.NewJsonResult(this._IUser.RoleLookup());
        }
        public ActionResult GetAllUsers()
        {
            return new WebMembership.MVC.NewJsonResult(this._IUser.GetAllUsers((HttpContext.Request.Form)));
        }
        public ActionResult DeleteUser(string Id)
        {
            var results = this._IUser.DeleteUser(Id, this.Log);
            return new WebMembership.MVC.NewJsonResult(results);
        }
        public ActionResult DevUserPermissions(string Id)
        {
            var results = this._IUser.DevUserPermissions(Id, this.Log);
            return new WebMembership.MVC.NewJsonResult(results);
        }
        public ActionResult DevDBFilter(string Id)
        {
            var results = this._IUser.DevDBFilter(Id, this.Log);
            return new WebMembership.MVC.NewJsonResult(results);
        }
        // GET: /SystemPage/User/
        public ActionResult UserIndex()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Views/Shared/_LayoutDevExtremeAjax.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_LayoutDevExtreme.cshtml";
            }

            return View();
        }
	}
}