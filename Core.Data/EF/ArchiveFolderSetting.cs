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
    
    public partial class ArchiveFolderSetting
    {
        public System.Guid ArchiveFolderSettingPK { get; set; }
        public string Name { get; set; }
        public string FolderPath { get; set; }
        public string ArchiveTo { get; set; }
        public Nullable<bool> IsFileOnly { get; set; }
        public Nullable<bool> IsArchiveByYear { get; set; }
        public Nullable<bool> IsArchiveByMonth { get; set; }
        public Nullable<bool> IsArchiveByWeek { get; set; }
        public Nullable<bool> IsArchiveByDay { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public Nullable<int> ServerGroup { get; set; }
        public string ServerName { get; set; }
        public string ServerDescription { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> NoOfMonthToDelete { get; set; }
    }
}
