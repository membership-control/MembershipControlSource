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
    
    public partial class Email_handler
    {
        public System.Guid UID { get; set; }
        public string Customer { get; set; }
        public string Message_Description { get; set; }
        public string Email_Address_From { get; set; }
        public string Email_Address_To { get; set; }
        public string Email_Subject { get; set; }
        public string Email_Content { get; set; }
        public string Attach_Filename { get; set; }
        public string Destination_Folder { get; set; }
        public string Insert_User { get; set; }
        public Nullable<System.DateTime> Insert_Date { get; set; }
        public string Updated_User { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public string status { get; set; }
        public Nullable<System.Guid> AID { get; set; }
        public Nullable<bool> IsRenameFileName { get; set; }
    }
}
