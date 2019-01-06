namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInsertUpdateDateRolePermissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRolePermissions", "InsertDate", c => c.DateTime());
            AddColumn("dbo.AspNetRolePermissions", "UpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRolePermissions", "InsertDate");
            DropColumn("dbo.AspNetRolePermissions", "UpdateDate");
        }
    }
}
