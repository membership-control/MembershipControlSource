using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class RegisterActivityGridDTO
    {
        public System.Guid ACT_PK { get; set; }
        public string ACT_Name { get; set; }
        public string Remark { get; set; }
        public string MBR_Name { get; set; }
        public string MBR_ChineseName { get; set; }
        public string MBR_Phone1 { get; set; }
        public string MBR_Phone2 { get; set; }
        public string MBR_WeChatNo { get; set; }
        public System.DateTime RegDate { get; set; }
    }
}
