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
    
    public partial class TGF_GI_ControlTower_Logging
    {
        public System.Guid PK { get; set; }
        public string Mode_ID { get; set; }
        public string Details { get; set; }
        public Nullable<System.DateTime> Insert_Date { get; set; }
        public string Insert_User { get; set; }
        public string Client { get; set; }
        public string Action { get; set; }
        public string SessionID { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Remark { get; set; }
    }
}
