namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUserPermissions",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    PermissionId = c.String(nullable: false, maxLength: 128),
                    Level = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.PermissionId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PermissionId);

            AddForeignKey("dbo.AspNetUserPermissions", "PermissionId", "dbo.AspNetPermissions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserPermissions", "PermissionId", "dbo.AspNetPermissions");
            DropForeignKey("dbo.AspNetUserPermissions", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserPermissions", new[] { "UserId" });
            DropIndex("dbo.AspNetUserPermissions", new[] { "PermissionId" });
            DropTable("dbo.AspNetUserPermissions");
        }
    }
}
