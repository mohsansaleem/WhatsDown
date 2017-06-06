using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using WhatsDown.Core.Domain;
using WhatsDown.Hubs;

[assembly: OwinStartupAttribute(typeof(WhatsDown.Startup))]
namespace WhatsDown
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            //var userId = MyCustomUserClass.FindUserId(request.User.Identity.Name);

            return HttpContext.Current.User.Identity.GetUserId();
        }
    }

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJSONP = true;
            hubConfiguration.EnableJavaScriptProxies = true;

            app.MapSignalR();

            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new MyIdProvider());
            //app.MapSignalR(MainHub);

        }
    }
}
