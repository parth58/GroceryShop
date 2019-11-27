namespace GroceryShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumberaddedtoOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PhoneNumber");
        }
    }
}
