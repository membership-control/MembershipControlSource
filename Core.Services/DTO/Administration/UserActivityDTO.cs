using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class UserActivityDTO
    {
        public System.Guid UAC_PK { get; set; }
        public int UAC_ID { get; set; }
        public System.Guid UAC_MBR_PK { get; set; }
        public System.Guid UAC_ACT_PK { get; set; }
        public string UAC_ACT_Name { get; set; }
        public string UAC_ACT_Type { get; set; }
        public Nullable<System.DateTime> UAC_ACT_From_Date { get; set; }
        public Nullable<System.DateTime> UAC_ACT_To_Date { get; set; }
        public string UAC_ACT_Remarks { get; set; }
        public System.DateTime UAC_RegDate { get; set; }
        public Nullable<System.DateTime> UAC_AttDate { get; set; }
        public string UAC_Current { get; set; }
        public Nullable<decimal> UAC_Fee { get; set; }
        public string UAC_Remarks { get; set; }
        public System.DateTime UAC_InsertDate { get; set; }
        public string UAC_InsertUser { get; set; }
        public Nullable<System.DateTime> UAC_UpdateDate { get; set; }
        public string UAC_UpdateUser { get; set; }
        public string UAC_DOC_ID { get; set; }
        public string UAC_MBR_Phone { get; set; }
        public string UAC_ACT_ID { get; set; }
    }
}
