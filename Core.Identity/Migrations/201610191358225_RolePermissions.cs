namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolePermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRolePermissions",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    PermissionId = c.String(nullable: false, maxLength: 128),
                    Level = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.RoleId, t.PermissionId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetPermissions", t => t.PermissionId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);

            CreateTable(
                "dbo.AspNetPermissions",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Area = c.String(nullable: false, maxLength: 512),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Area, unique: true, name: "AreaIndex");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetRolePermissions", "PermissionId", "dbo.AspNetPermissions");
            DropForeignKey("dbo.AspNetRolePermissions", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetRolePermissions", new[] { "RoleId" });
            DropIndex("dbo.AspNetRolePermissions", new[] { "PermissionId" });
            DropIndex("dbo.AspNetPermissions", "AreaIndex");
            DropTable("dbo.AspNetPermissions");
            DropTable("dbo.AspNetRolePermissions");
        }
    }
}
