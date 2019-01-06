using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  WebMembership.Areas.AdministrationPage.Models;
using System.IO;
using System.Configuration;
using WebMembership.CustomFilters;
using Core.Identity.Models;

namespace WebMembership.Areas.Administration.Controllers
{
    //[CustomAuthorize]
    //[AuthLog(Area = "030400000000", PermissionLevel = PermissionLevel.Read)]
    public class MembershipController : WebMembership.Controllers.BaseController
    {
         private IMember _IMember;

         public MembershipController(IMember iMember)
        {
            this._IMember = iMember;
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            var results = this._IMember.DevPageAll(HttpContext.Request.Form); //this.Log);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        [HttpPost]
        public ActionResult IsIDAvailable(string id)
        {
            var results = this._IMember.CheckID(id);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        [HttpPost]
        public ActionResult Upload(string name)
        {
            bool result = false;
            if (Request.Files.Count != 0)
            {
                string fileName = "";

                //if (Request.Browser.Browser.Contains("InternetExplorer"))
                //    fileName = String.Format("{0}{1}",name.Replace("-",""),System.IO.Path.GetFileName(Request.Files[0].FileName));
                //else
                    fileName = String.Format("{0}{1}", name.Replace("-", ""), Path.GetExtension(Request.Files[0].FileName));

                string filePath = string.Format("~/Images/uploads/{0}", fileName);
                Request.Files[0].SaveAs(Server.MapPath(filePath));
                result = true;
            }

            //return View();
            return Json(result, JsonRequestBehavior.DenyGet);
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