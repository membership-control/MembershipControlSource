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
    
    public partial class ImportListenerToken
    {
        public Nullable<System.Guid> Setting_PK { get; set; }
        public System.Guid PK { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
        public string insert_user { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public string update_user { get; set; }
        public string FileFullName { get; set; }
        public Nullable<System.Guid> batch_id { get; set; }
    }
}
