using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Services.DTO.Generic
{
    public class NavbarViewModel
    {
        public IEnumerable<NavbarParent> Categories { get; set; }
        public IEnumerable<NavbarChild> Pages { get; set; }
    }

    public class NavbarParent
    {
        public string Category { get; set; }
    }

    public class NavbarChild
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string ImageClass { get; set; }
        public string Category { get; set; }
        public string PermissionId { get; set; }
        public int Seq { get; set; }
    }
}
