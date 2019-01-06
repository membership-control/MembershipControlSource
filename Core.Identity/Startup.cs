using Owin;
using Microsoft.Owin;
namespace Core.Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
            var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR("/signalr", hubConfiguration);
        }
    }
}
