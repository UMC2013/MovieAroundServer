using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieAroundServer.Startup))]
namespace MovieAroundServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
