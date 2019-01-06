using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.SystemPage
{
    public class UserHeaderDTO
    {
         public string Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        public int? UserSecurity { get; set; }

        public bool? UserSec { get { return (UserSecurity == 1 ? true : false); } }
        public int? DBFilter { get; set; }

        public bool? DBFil { get { return (DBFilter == 1 ? true : false); } }
        public int? totalcount { get; set; }
        public string EmailAddress { get; set; }
    }
}
