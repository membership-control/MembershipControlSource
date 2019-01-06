using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class ActivityDTO
    {
        public System.Guid ACT_PK { get; set; }
        public string ACT_ID { get; set; }
        public string ACT_Type { get; set; }
        public string ACT_Category { get; set; }
        public string ACT_Name { get; set; }
        public Nullable<System.DateTime> ACT_FromDate { get; set; }
        public Nullable<System.DateTime> ACT_ToDate { get; set; }
        public string ACT_Location { get; set; }
        public string ACT_Address { get; set; }
        public string ACT_Current { get; set; }
        public Nullable<decimal> ACT_Fee { get; set; }
        public string ACT_Status { get; set; }
        public Nullable<int> ACT_MaxSeat { get; set; }
        public string ACT_Remarks { get; set; }
        public int ACT_MemberTypeReq { get; set; }
        public System.DateTime ACT_InsertDate { get; set; }
        public string ACT_InsertUser { get; set; }
        public Nullable<System.DateTime> ACT_UpdateDate { get; set; }
        public string ACT_UpdateUser { get; set; }
        public string ACT_DOC_ID { get; set; }
        public string ACT_Flex1 { get; set; }
        public string ACT_Flex2 { get; set; }
        public string ACT_Flex3 { get; set; }
        public string ACT_Flex4 { get; set; }
        public string ACT_Flex5 { get; set; }
    }
}
