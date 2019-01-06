using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Identity.Models;
using WebMembership.CustomFilters;
using WebMembership.Areas.SystemPage.Models;

namespace WebMembership.Areas.SystemPage.Controllers
{
    [CustomAuthorize]
    [AuthLog(Area = "010300000000", PermissionLevel = PermissionLevel.Read)]
    public class ServiceStatusController : WebMembership.Controllers.BaseController //Controller
    {
        private IServiceStatus _iServiceStatus;


        public ServiceStatusController(IServiceStatus iServiceStatus)
        {
            this._iServiceStatus = iServiceStatus;
        }

        // GET: AdminPage/ListenerMaster
        public ActionResult ServiceStatusIndex()
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

        //public ActionResult ServiceStatusLoad()
        //{
        //    var results = this._iServiceStatus.GetServiceDetail();
        //    return new WebControlTower.MVC.NewJsonResult(results);

        //}
      
        public ActionResult ServiceStatusLoad()
        {
  //          var results = this._iServiceStatus.DevPageAll(HttpContext.Request.Form, this.Log);
//            return new WebControlTower.MVC.NewJsonResult(results);
            ServiceStatusPageModel PageModel = new ServiceStatusPageModel();
            PageModel.Details = _iServiceStatus.GetAllData();
            Core.Infrastructure.Dev.DevResponse dr = new Core.Infrastructure.Dev.DevResponse();
            dr.data = PageModel.Details;
            dr.totalCount = PageModel.Details.Count();
//            return new WebControlTower.MVC.NewJsonResult(dr, JsonRequestBehavior.AllowGet);
            return new WebMembership.MVC.NewJsonResult(PageModel, JsonRequestBehavior.AllowGet);
        }
          [AuthLog(Area = "010301000000", PermissionLevel = PermissionLevel.All)]
        public ActionResult StartService(string setting_pk, string action)
        {
            if (action == "TRUE")
                return new WebMembership.MVC.NewJsonResult(_iServiceStatus.StartService(setting_pk, true, this.Log));
            else
                return new WebMembership.MVC.NewJsonResult(_iServiceStatus.StartService(setting_pk, false, this.Log));
        }
    }
}