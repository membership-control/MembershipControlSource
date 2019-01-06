using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Services.Interface;
using Core.Services.DTO.Generic;

namespace WebMembership.Controllers
{
        //[Authorize]
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

            var result = new NavbarViewModel(); //this._iNavbar.GetAllMenu(userManager, roleManager, username);
            result.Categories = new List<NavbarParent>()
            {
                new NavbarParent() { Category = "Administration" }
            };
            result.Pages = new List<NavbarChild>()
            {
                new NavbarChild() {
                    Id = "1316E944-EA36-4D4B-ACF1-1E36B7F6FCC2",
                    Name = "Register",
                    Controller = "Register",
                    Action = "Index",
                    Area = "AdministrationPage",
                    ImageClass = "fa fa-calendar-check-o",
                    Category = "Administration",
                    PermissionId = "030100000000",
                    Seq = 1
                },
                new NavbarChild() {
                    Id = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA",
                    Name = "Membership",
                    Controller = "Membership",
                    Action = "Index",
                    Area = "AdministrationPage",
                    ImageClass = "fa fa-user-plus",
                    Category = "Administration",
                    PermissionId = "030200000000",
                    Seq = 2
                },
                new NavbarChild() {
                    Id = "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB",
                    Name = "Activity",
                    Controller = "Activity",
                    Action = "Index",
                    Area = "AdministrationPage",
                    ImageClass = "fa fa-ticket",
                    Category = "Administration",
                    PermissionId = "030300000000",
                    Seq = 3
                }
            };

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

            //var username = this.UserName;
            //var userManager = this.UserManager;
            //var roleManager = this.RoleManager;

            var result = new NavbarViewModel();////this._iNavbar.GetAllMenu(userManager, roleManager, username);
            //result.Categories = new List<NavbarParent>()
            //{
            //    new NavbarParent() { Category = "Administration" }
            //};
            //result.Pages = new List<NavbarChild>()
            //{
            //    new NavbarChild() {
            //        Id = "1316E944-EA36-4D4B-ACF1-1E36B7F6FCC2",
            //        Name = "Register",
            //        Controller = "Register",
            //        Action = "Index",
            //        Area = "AdministrationPage",
            //        ImageClass = "fa fa-calendar-check-o",
            //        Category = "Administration",
            //        PermissionId = "030100000000",
            //        Seq = 1
            //    }
            //};
            return View(result);
        }

    }
}