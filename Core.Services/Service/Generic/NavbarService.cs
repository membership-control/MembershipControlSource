using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.Interface;
using Core.Services.DTO.Generic;
using Core.Identity.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Core.Services.Service
{
    public class NavbarService : INavbar
    {
        public NavbarService()
        {
        }


        public NavbarViewModel GetAllMenu(Identity.ApplicationUserManager user_manager, Identity.ApplicationRoleManager role_manager, string username)
        {
            var user = user_manager.FindByName(username);

            var userRoleIds = (from r in user.Roles select r.RoleId);

            var userRoles = (from id in userRoleIds
                             let r = role_manager.FindById(id)
                             select r).ToList();

            var result_model = new NavbarViewModel();
            result_model.Categories = new List<NavbarParent>();
            result_model.Pages = new List<NavbarChild>();
            using (var context = new ApplicationDbContext())
            {
                var all_navbars = context.NavBars.ToList();
                var config = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
                var mapper = config.CreateMapper();
                if (userRoles.Any(r => r.Name == "Admin"))
                {
                    //CHONG : temporary use hardcode Case When sorting for Category
                    result_model.Categories = all_navbars.OrderBy(menu => menu.Category.Contains("System") ? "A1" : menu.Category).ToList()
                                            .Select(a => a.Category).GroupBy(t => t)
                                            .Select(g => mapper.Map<NavbarParent>(new { Category = g.Key }))
                                            .ToList();
                    result_model.Pages = all_navbars.Select(a => mapper.Map<NavbarChild>(a)).OrderBy(menu => menu.Seq).ToList();
                }
                else
                {
                    var permission_ids = user.UserPermissions.Select(p => p.PermissionId).ToList();
                    userRoles.ForEach(role =>
                    {
                        permission_ids = role.RolePermissions.Select(p => p.PermissionId).Union(permission_ids).ToList();
                    });
//                    result_model.Pages = all_navbars.Where(menu => permission_ids.Contains(menu.PermissionId) || string.IsNullOrEmpty(menu.PermissionId))
                        result_model.Pages = all_navbars.Where(menu => permission_ids.Contains(menu.PermissionId) )
                                    .Select(a => mapper.Map<NavbarChild>(a))
                                    .OrderBy(menu => menu.Seq)
                                    .ToList();
                    result_model.Categories = result_model.Pages.OrderBy(menu => menu.Category.Contains("System") ? "A1" : menu.Category).ToList()
                                            .Select(a => a.Category).GroupBy(t => t)
                                            .Select(g => mapper.Map<NavbarParent>(new { Category = g.Key }))
                                            .ToList();
                }
            }

            return result_model;

        }
    }
}
