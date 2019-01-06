namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInsertUpdateDateUserPermissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserPermissions", "InsertDate", c => c.DateTime());
            AddColumn("dbo.AspNetUserPermissions", "UpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUserPermissions", "InsertDate");
            DropColumn("dbo.AspNetUserPermissions", "UpdateDate");
        }
    }
}
