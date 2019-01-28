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
        public Nullable<System.Guid> MBR_PK { get; set; }
        public string MBR_Name { get; set; }
        public string MBR_Phone1 { get; set; }
        public string MBR_Email { get; set; }
        public int MBR_Type { get; set; }
        public string UAC_ACT_Remarks { get; set; }
        public bool IsCheckin { get; set; }
        public bool IsEmail { get; set; }
    }
}
