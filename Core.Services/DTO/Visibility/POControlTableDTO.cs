using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class POControlTableDTO
    {
        public string ORDERNO { get; set; }
        public string ORDER_LINENO { get; set; }
        public int ORDER_SPLIT { get; set; }
        public string BATCH_ID { get; set; }
        public string SOURCE_FILE_NAME { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string PRODUCT { get; set; }
        public string CUSTOMER_ID { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
        public string PRE_PROCESS_STATUS { get; set; }
        public string STAGING_STATUS { get; set; }
        public string FILE_GENERATED { get; set; }
        public string TRANSFER_STATUS { get; set; }
        public string IN_EDIENTERPRISE { get; set; }
        public Nullable<System.Guid> EDI_ORD_LINE_PK { get; set; }
        public Nullable<System.DateTime> EDI_LAST_EDIT_TIME { get; set; }
        public string COMMENT { get; set; }
        public Nullable<System.DateTime> FTP_RECEIVE_DATE { get; set; }
        public string FTP_VS_RECEIVE { get; set; }
        public string SEND_TO_FOLDER { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE_UTC { get; set; }
    }

    public class POControlTableMasterDTO
    {
        public string CUSTOMER_ID { get; set; }
        public string ORDERNO { get; set; }
        public int ORDER_SPLIT { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE_UTC { get; set; }
        public string SUPPLIER_CODE { get; set; }
        //public string ORDER_STATUS { get; set; }
        public string IN_EDIENTERPRISE { get; set; }
        public Nullable<System.DateTime> EDI_LAST_EDIT_TIME { get; set; }
        public Nullable<System.DateTime> FTP_RECEIVE_DATE { get; set; }
        public string FTP_VS_RECEIVE { get; set; }
        //public List<POControlTableDetailDTO> Details { get; set; }
    }

    public class POControlTableDetailDTO
    {
        //public string ORDERNO { get; set; }
        //public int ORDER_SPLIT { get; set; }
        //public string CUSTOMER_ID { get; set; }
        public string BATCH_ID { get; set; }
        //public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
        public string ORDER_LINENO { get; set; }
        public string PRODUCT { get; set; }
        public string PRE_PROCESS_STATUS { get; set; }
        public string STAGING_STATUS { get; set; }
        public string TRANSFER_STATUS { get; set; }
        public Nullable<System.DateTime> FTP_RECEIVE_DATE { get; set; }
        public string SOURCE_FILE_NAME { get; set; }
        public string FILE_GENERATED { get; set; }
        public string COMMENT { get; set; }
        public bool REPROCESS { get; set; }
        //public string SEND_TO_FOLDER { get; set; }
    }
}
