namespace ChatClientApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOnLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OnLine", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "OnLine");
        }
    }
}
