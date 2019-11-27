namespace GroceryShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zxcvbnm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "isRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "isRead");
        }
    }
}
