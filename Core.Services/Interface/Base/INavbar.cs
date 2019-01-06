using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.DTO.Generic;
using Core.Identity;

namespace Core.Services.Interface
{
    public interface INavbar
    {
        NavbarViewModel GetAllMenu(ApplicationUserManager user_manager, ApplicationRoleManager role_manager, string username);
    }
}
