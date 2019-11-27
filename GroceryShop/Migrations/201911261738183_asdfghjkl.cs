namespace GroceryShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdfghjkl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "FirstName");
        }
    }
}
