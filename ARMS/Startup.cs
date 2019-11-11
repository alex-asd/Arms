using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ARMS.Startup))]
namespace ARMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
