using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Services.Interface;
using WebMembership.Areas.AdministrationPage.Models;
using System.IO;
using System.Configuration;
using WebMembership.CustomFilters;
using Core.Identity.Models;

namespace WebMembership.Areas.Administration.Controllers
{
    [CustomAuthorize]
    //[AuthLog(Area = "030500000000", PermissionLevel = PermissionLevel.Read)]
    public class ActivityController : WebMembership.Controllers.BaseController
    {
         private IActivity _Iactivity;

         public ActivityController(IActivity iActivity)
        {
            this._Iactivity = iActivity;
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            var results = _Iactivity.DevPageAll(HttpContext.Request.Form, this.Log);
            //return new WebMembership.MVC.NewJsonResult(results);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        [HttpPost]
        public ActionResult IsIDAvailable(string id)
        {
            var results = _Iactivity.CheckID(id);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult Index(string icon)
        {
            ViewData["Icon"] = icon;
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