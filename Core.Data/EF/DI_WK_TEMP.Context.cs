﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DI_WK_TEMPEntities : DbContext
    {
        public DI_WK_TEMPEntities()
            : base("name=DI_WK_TEMPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MEM_Activity> MEM_Activity { get; set; }
        public virtual DbSet<MEM_SysLog> MEM_SysLog { get; set; }
        public virtual DbSet<MEM_Membership> MEM_Membership { get; set; }
        public virtual DbSet<MEM_UserActivity> MEM_UserActivity { get; set; }
    }
}
