using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(btl.Startup))]
namespace btl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
