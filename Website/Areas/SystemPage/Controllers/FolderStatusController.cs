using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMembership.Areas.SystemPage.Models;
using Core.Identity.Models;
using WebMembership.CustomFilters;

namespace WebMembership.Areas.SystemPage.Controllers
{
    [CustomAuthorize]
     public class FolderStatusController : WebMembership.Controllers.BaseController //Controller
    {
        private IFolderStatus _iFolderStatus;

        public FolderStatusController(IFolderStatus iFolderStatus)
        {
            this._iFolderStatus = iFolderStatus;
        }

         [AuthLog(Area = "010500000000", PermissionLevel = PermissionLevel.Read)]
        public ActionResult FolderStatusIndex()
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
        public ActionResult FolderStatusLoad()
        {
            FolderStatusPageModel PageModel = new FolderStatusPageModel();
            PageModel.Details = _iFolderStatus.GetAllData();
            Core.Infrastructure.Dev.DevResponse dr = new Core.Infrastructure.Dev.DevResponse();
            //dr.data = PageModel.Details;
            //dr.totalCount = PageModel.Details.Count();
            return new WebMembership.MVC.NewJsonResult(PageModel, JsonRequestBehavior.AllowGet);
        }
	}
}