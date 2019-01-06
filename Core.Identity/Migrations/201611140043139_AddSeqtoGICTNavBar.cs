namespace Core.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeqtoGICTNavBar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetGICTNavBar", "Seq", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetGICTNavBar", "Seq");
        }
    }
}
