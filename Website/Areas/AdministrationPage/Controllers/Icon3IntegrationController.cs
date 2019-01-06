using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using Core.Identity.Models;
using Core.Services.Interface;
using Core.Services.DTO.Visibility;
using WebMembership.CustomFilters;

namespace WebMembership.Areas.Administration.Controllers
{
    [CustomAuthorize]
    [AuthLog(Area = "030600000000", PermissionLevel = PermissionLevel.Read)]
    public class Icon3IntegrationController : WebMembership.Controllers.BaseController
    {
        private IIcon3Integration _iIcon3Integration;

        public Icon3IntegrationController(IIcon3Integration icon3_integration_control)
        {
            this._iIcon3Integration = icon3_integration_control;
        }

        // GET: VisibilityPage/Icon3Integration
        public ActionResult Index()
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

        public ActionResult LoadChartDataset(string range)
        {
            var results = this._iIcon3Integration.DevChartDataset(range, this.Log);
            return new WebMembership.MVC.NewJsonResult(results.data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadAll(string range)
        {
            if (string.IsNullOrEmpty(range)) range = "24 Hours";
            var results = this._iIcon3Integration.DevPageAll(HttpContext.Request.Form, range, this.Log);

            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult LoadLog(string targetFile, string targetFileType = null)
        {
            string path = "";
            string unc = "";
#if UAT
            path = ConfigurationManager.AppSettings["LogPathUAT"];
            unc = ConfigurationManager.AppSettings["LogPathUAT"];
#endif
#if PROD
            path = ConfigurationManager.AppSettings["LogPathPROD"];
            unc = ConfigurationManager.AppSettings["LogPathPROD"];
#endif

            path = targetFile.ToLower().Replace(@"c:/", path);
            path = path.Replace(@"/", @"\");
            path = Path.GetDirectoryName(path) + @"\";


            var results = this._iIcon3Integration.readUNC(targetFile, path, unc);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [AuthLog(Area = "030601000000", PermissionLevel = PermissionLevel.All)]
        public ActionResult ReprocessFile(string targetFile)
        {
            string path = "";
#if UAT
            path = ConfigurationManager.AppSettings["connectListenerUAT"];
#endif
#if PROD
            path = ConfigurationManager.AppSettings["connectListenerPROD"];
#endif
            path = String.Concat(path, @"\Inbound\iCON3\");
            //path = path.Replace(@"\GlobalIntegration", "");
            var log = this.Log;
            log.Remark = targetFile;
            var results = this._iIcon3Integration.ReprocessFile(targetFile, path, log);
            return Json(results);
        }
    }
}