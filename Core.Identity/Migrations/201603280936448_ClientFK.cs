namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetClients", "ApplicationUser_Id", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.AspNetClients", name: "ApplicationUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.AspNetClients", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
            AddForeignKey("dbo.AspNetClients", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetClients", "UserId", "dbo.AspNetUsers");
            RenameIndex(table: "dbo.AspNetClients", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.AspNetClients", name: "UserId", newName: "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetClients", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
