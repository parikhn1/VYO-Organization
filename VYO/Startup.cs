using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VYO.Startup))]
namespace VYO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
