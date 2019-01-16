using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class RegisterActivityMasterDTO
    {
        public System.Guid ACT_PK { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public string text { get; set; }
        public string ACT_Type { get; set; }
        public string ACT_Status { get; set; }
        public string ACT_Current { get; set; }
        public Nullable<decimal> ACT_Fee { get; set; }
        public string ACT_Remarks { get; set; }
        public Nullable<System.Guid> MBR_PK { get; set; }
        public string MBR_Name { get; set; }
        public string MBR_Phone1 { get; set; }
        public System.DateTime UAC_RegDate { get; set; }
        Nullable<System.DateTime> _attdate;
        public Nullable<System.DateTime> UAC_AttDate {
            get { return _attdate ?? DateTime.Now; }
            set { _attdate = value; }
        }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
    }
}
