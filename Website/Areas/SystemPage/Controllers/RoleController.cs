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

namespace WebMembership.Areas.SystemPage.Controllers
{
    [CustomAuthorize]
    public class RoleController : BaseController
    {
        private Core.Services.Interface.IRole _IRole;

        public RoleController(Core.Services.Interface.IRole iIRole)
        {
            this._IRole = iIRole;
        }

        // GET: SystemPage/Role
        [AuthLog(Area = "010100000000", PermissionLevel = PermissionLevel.Read)]
        public ActionResult RoleIndex()
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

        public ActionResult DevPageData()
        {
            var results = this._IRole.DevPageAll(HttpContext.Request.Form, this.Log);

            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult DevRolePermissions(string Id)
        {
            var results = this._IRole.DevRolePermissions(Id, this.Log);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult DevSavePermissions_Test(string pid, string json)
        {
            return new WebMembership.MVC.NewJsonResult("");
        }


    }
}