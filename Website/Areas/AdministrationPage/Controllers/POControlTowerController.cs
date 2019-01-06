using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using Core.Services.Interface;
using Core.Services.DTO.Visibility;
using WebMembership.Areas.AdministrationPage.Models;
using WebMembership.CustomFilters;
using Core.Identity.Models;

namespace WebMembership.Areas.Administration.Controllers
{
    [CustomAuthorize]
    [AuthLog(Area = "030300000000", PermissionLevel = PermissionLevel.Read)]
    public class POControlTowerController : WebMembership.Controllers.BaseController
    {
        private IPOControl _iPOControl;

        public POControlTowerController(IPOControl po_control)
        {
            this._iPOControl = po_control;
        }

        // GET: VisibilityPage/POControlTower
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

        public ActionResult LoadAll(string range)
        {
            if (string.IsNullOrEmpty(range)) range = "7 Days";
            var results = this._iPOControl.DevPageAll(HttpContext.Request.Form, range, this.Log);
            
            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult LoadDetails(string range, string customerid, string orderno, int ordersplit, string suppliercode)
        {
            var results = this._iPOControl.DevPageDetails(range, customerid, orderno, ordersplit, suppliercode);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        public ActionResult LoadChartDataByCustomer(string range)
        {
            var results = this._iPOControl.DevChartDataByCustomer(range, this.Log);
            return new WebMembership.MVC.NewJsonResult(results.data, JsonRequestBehavior.AllowGet);//Json(results.data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChartDataFTPVsProcessing(string range)
        {
            var results = this._iPOControl.DevChartDataFTPVsProcessing(range, this.Log);
            return Json(results.data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChartDataOrderStatus(string range)
        {
            var results = this._iPOControl.DevChartDataOrderStatus(range, this.Log);
            return new WebMembership.MVC.NewJsonResult(results.data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadLog(string targetFile, string targetFileType = null)
        {
            string path = "";
            string unc = "";
#if UAT
            path = ConfigurationManager.AppSettings["connectUAT"];
            unc = ConfigurationManager.AppSettings["connectUAT"];
#endif
#if PROD
                    path = ConfigurationManager.AppSettings["connectPROD"];
            unc = ConfigurationManager.AppSettings["connectPROD"];
#endif

            path = targetFile.ToLower().Replace(@"c:\di", path);
            path = Path.GetDirectoryName(path) + @"\";


            var results = this._iPOControl.readUNC(targetFile, path, unc);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [AuthLog(Area = "030301000000", PermissionLevel = PermissionLevel.All)]
        public ActionResult ReprocessFile(string targetFile, string sendToFolder, string batchid)
        {
            string listenerUNC = "";
            string path = "";
            string unc = "";
#if UAT
            listenerUNC = ConfigurationManager.AppSettings["connectListenerUAT"];
            listenerUNC = listenerUNC.ToLower().Replace(@"\uat\globalintegration", "");
            path = ConfigurationManager.AppSettings["connectUAT"];
            unc = ConfigurationManager.AppSettings["connectUAT"];
#endif
#if PROD
            listenerUNC = ConfigurationManager.AppSettings["connectListenerPROD"];
            listenerUNC = listenerUNC.ToLower().Replace(@"\prod\globalintegration", "");
            path = ConfigurationManager.AppSettings["connectPROD"];
            unc = ConfigurationManager.AppSettings["connectPROD"];
#endif
            path = targetFile.ToLower().Replace(@"c:\di", path);
            sendToFolder = sendToFolder.ToLower().Replace(@"d:", listenerUNC);
            var log = this.Log;
            log.Remark = batchid;
            var results = this._iPOControl.ReprocessFile(path, sendToFolder, unc, log);
            return Json(results);
        }

        public ActionResult LoadSelectBoxClientData()
        {
            var results = this._iPOControl.DevSelectBoxClientData();
            return Json(results.data);
        }

        //[AuthLog(Area = "POControlTower", PermissionLevel = PermissionLevel.All)]
        //public ActionResult TriggerPOIntegrationJob(string clientname)
        //{
        //    var results = this._iPOControl.CallDI(clientname);
        //    return Json(results);
        //}

    }
}