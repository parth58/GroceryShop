using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroceryShop.Startup))]
namespace GroceryShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
