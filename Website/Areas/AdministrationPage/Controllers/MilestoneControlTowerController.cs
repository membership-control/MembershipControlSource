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
    public class MilestoneControlTowerController : WebMembership.Controllers.BaseController
    {
        // GET: VisibilityPage/MilestoneControlTower
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
    }
}