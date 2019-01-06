namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GICTNavBar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetGICTNavBar",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                    Controller = c.String(maxLength: 256),
                    Action = c.String(maxLength: 256),
                    Area = c.String(maxLength: 256),
                    ImageClass = c.String(maxLength: 256),
                    Category = c.String(nullable: false, maxLength: 256),
                    PermissionId = c.String(nullable: true, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetPermissions", t => t.PermissionId, cascadeDelete: true)
                .Index(t => new { t.Name, t.Category }, name: "IdxCategoryName", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetGICTNavBar", "PermissionId", "dbo.AspNetPermissions");
            DropIndex("dbo.AspNetGICTNavBar", "IdxCategoryName");
            DropTable("dbo.AspNetGICTNavBar");
        }
    }
}
