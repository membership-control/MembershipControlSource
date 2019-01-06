using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class MemberDTO
    {
        public System.Guid MBR_PK { get; set; }
        public int MBR_ID { get; set; }
        public int MBR_Type { get; set; }
        public string MBR_Name { get; set; }
        public string MBR_ChineseName { get; set; }
        public string MBR_Phone1 { get; set; }
        public string MBR_Phone2 { get; set; }
        public string MBR_Address1 { get; set; }
        public string MBR_Address2 { get; set; }
        public string MBR_CountryCode { get; set; }
        public string MBR_CountryName { get; set; }
        public Nullable<int> MBR_Age { get; set; }
        public string MBR_CompanyName { get; set; }
        public string MBR_Occupations { get; set; }
        public string MBR_Skillset { get; set; }
        public string MBR_Professional { get; set; }
        public string MBR_Valuable { get; set; }
        public string MBR_Networking { get; set; }
        public string MBR_Parhnership { get; set; }
        public string MBR_SupportInGroup { get; set; }
        public bool MBR_Agreement { get; set; }
        public string MBR_ReferBy { get; set; }
        public string MBR_MemberID { get; set; }
        public string MBR_GroupID { get; set; }
        public Nullable<System.DateTime> MBR_EffectiveDate { get; set; }
        public Nullable<System.DateTime> MBR_ExpiredDate { get; set; }
        public string MBR_Remarks { get; set; }
        public bool MBR_IsEnable { get; set; }
        public System.DateTime MBR_InsertDate { get; set; }
        public string MBR_InsertUser { get; set; }
        public Nullable<System.DateTime> MBR_UpdateDate { get; set; }
        public string MBR_UpdateUser { get; set; }
        public string MBR_QRCode { get; set; }
        public string MBR_WeChatNo { get; set; }
        public string MBR_DOC_ID { get; set; }
        public string MBR_Flex1 { get; set; }
        public string MBR_Flex2 { get; set; }
        public string MBR_Flex3 { get; set; }
        public string MBR_Flex4 { get; set; }
        public string MBR_Flex5 { get; set; }
    }
}
