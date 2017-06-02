using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhatsDown.Startup))]
namespace WhatsDown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
