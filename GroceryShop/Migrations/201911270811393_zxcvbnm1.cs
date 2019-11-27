namespace GroceryShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zxcvbnm1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "CreatedOn");
        }
    }
}
