//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class TGF_PO_CONTROL_TABLE
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
        public Nullable<System.DateTime> RECEIVE_DATE_UTC { get; set; }
    }
}
