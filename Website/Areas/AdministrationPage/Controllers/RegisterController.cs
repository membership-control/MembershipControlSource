using Core.Services.Interface;
using Core.Services.DTO.Generic;
using WebMembership.Areas.AdministrationPage.Models;
using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebMembership.CustomFilters;
using Core.Identity.Models;


namespace WebMembership.Areas.Administration.Controllers
{
    //[CustomAuthorize]
    public class RegisterController : WebMembership.Controllers.BaseController
    {
        private IRegister _iRegister;
        private RegisterViewModel _vm;
        
        public RegisterController(IRegister RegisterInterface)
        {
            //this._iALVWHistory = ALVWHistory;
            this._iRegister = RegisterInterface;
            _vm = new RegisterViewModel();
            //_vm.ServicesSource = this._iALVWHistory.GetAll().Select(s => s.SERVICE).Distinct().ToArray();
        }

        //// GET: Visibility/DIProcess
        //[AuthLog(Area = "030100000000", PermissionLevel = PermissionLevel.Read)]
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

            return View(_vm);
        }

        //[HttpPost]
        public ActionResult LoadAll()
        {
            //var results = this._iALVWHistory.DevPageAll(HttpContext.Request.Form);
            //return new WebMembership.MVC.NewJsonResult(results);
            var results = this._iRegister.DevPageAll(HttpContext.Request.Form);
            return new WebMembership.MVC.NewJsonResult(results);
        }

        [HttpPost]
        public async Task<ActionResult> UploadForms()
        {
            Core.Infrastructure.Dev.DevResponse result = null;
            
            if (Request.Files.Count != 0)
            {
                BinaryReader b = new BinaryReader(Request.Files[0].InputStream);
                byte[] binaryData = b.ReadBytes(Request.Files[0].ContentLength);
                HTMLEmailViewModel email_model = new HTMLEmailViewModel();
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/template/Email/QR_email_template.html")))
                {
                    email_model.Body = reader.ReadToEnd();
                }

                    result = await this._iRegister.UploadForm(binaryData, true, email_model);
            }
            
            //return View();
            return new WebMembership.MVC.NewJsonResult(result, JsonRequestBehavior.DenyGet);
        }

        //protected async Task<ActionResult> SendEmail(object data)
        //{
        //    bool result = false;
        //    HTMLEmailViewModel email_model = new HTMLEmailViewModel();

        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/template/Email/QR_email_template.html")))
        //    {
        //        email_model.Body = reader.ReadToEnd();
        //    }

        //    await this._iRegister.SendFormattedHTMLEmailAsync(email_model);
        //    result = true;

        //    return Json(result, JsonRequestBehavior.DenyGet);
        //}
        
    }
}