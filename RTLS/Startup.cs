using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RTLS.Startup))]
namespace RTLS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
