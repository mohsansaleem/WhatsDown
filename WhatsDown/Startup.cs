﻿using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using WhatsDown.Hubs;

[assembly: OwinStartupAttribute(typeof(WhatsDown.Startup))]
namespace WhatsDown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

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
