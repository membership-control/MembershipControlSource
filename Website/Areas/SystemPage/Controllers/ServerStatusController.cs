using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMembership.Areas.SystemPage.Models;
using Core.Services.Interface;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using Core.Identity.Models;
using WebMembership.CustomFilters;

namespace WebMembership.Areas.SystemPage.Controllers
{
    [CustomAuthorize]
    public class ServerStatusController : WebMembership.Controllers.BaseController //Controller
    {
        private IServerStatus  _iServerStatus;

        public ServerStatusController(IServerStatus iServerStatus)
        {
            this._iServerStatus = iServerStatus;
        }
        // GET: SystemPage/ServerStatus
        public ActionResult Chart()
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
         [AuthLog(Area = "010400000000", PermissionLevel = PermissionLevel.Read)]
        public ActionResult ServerStatus()
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
           

        public ActionResult ServerStatusLoad()
        {
            ServerStatusPageModel PageModel = new ServerStatusPageModel();
            PageModel.Details = _iServerStatus.GetAllData();
            Core.Infrastructure.Dev.DevResponse dr = new Core.Infrastructure.Dev.DevResponse();
            //dr.data = PageModel.Details;
            //dr.totalCount = PageModel.Details.Count();
            return new WebMembership.MVC.NewJsonResult(PageModel, JsonRequestBehavior.AllowGet);

        }
       
    }
}