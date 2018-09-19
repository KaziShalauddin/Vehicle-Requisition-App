using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VehicleManagementApp.Startup))]
namespace VehicleManagementApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
