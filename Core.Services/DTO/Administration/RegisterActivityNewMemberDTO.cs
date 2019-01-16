using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class RegisterActivityNewMemberDTO
    {
        public System.Guid ACT_PK { get; set; }
        public string MBR_Name { get; set; }
        public string MBR_Phone1 { get; set; }
        public string MBR_Email { get; set; }
        public bool IsCheckin { get; set; }
        public bool IsEmail { get; set; }
    }
}
