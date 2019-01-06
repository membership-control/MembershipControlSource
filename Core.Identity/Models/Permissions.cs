using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Models
{
    [Flags]
    public enum PermissionLevel
    {
        None = 0,
        Read = 1,
        Create = 1<<1,
        Update = 1<<2,
        Delete = 1<<3,
        All = 16
    }
}
