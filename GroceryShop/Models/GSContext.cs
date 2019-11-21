namespace GroceryShop.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class GSContext : DbContext
    {
        // Your context has been configured to use a 'GSContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GroceryShop.Models.GSContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'GSContext' 
        // connection string in the application configuration file.
        public GSContext()
            : base("name=GSContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<Product> Products { get; set; }
         public virtual DbSet<Category> Categories { get; set; }
       
    }

   
}