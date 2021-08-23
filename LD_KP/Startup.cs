using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LD_KP.Startup))]
namespace LD_KP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
