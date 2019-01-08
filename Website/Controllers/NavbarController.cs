using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Services.Interface;
using Core.Services.DTO.Generic;

namespace WebMembership.Controllers
{
    [Authorize]
    public class NavbarController : BaseController //Controller
    {
        private INavbar _iNavbar;

        public NavbarController(INavbar iNavbar)
        {
            this._iNavbar = iNavbar;
        }

        // GET: Navbar
        public ActionResult NavBarIndex()
        {
            var username = this.UserName;
            var userManager = this.UserManager;
            var roleManager = this.RoleManager;

            var result = this._iNavbar.GetAllMenu(userManager, roleManager, username);

            return PartialView("_MenuPartial", result);
        }

        public ActionResult LandingPage()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Views/Shared/_LayoutDevExtremeAjax.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_LayoutDevExtreme.cshtml";
            }
            //Chong Membership Temp - maybe no more use Landing Page
            //var username = this.UserName;
            //var userManager = this.UserManager;
            //var roleManager = this.RoleManager;

            var result = new NavbarViewModel();////this._iNavbar.GetAllMenu(userManager, roleManager, username);
            
            return View(result);
        }

    }
}