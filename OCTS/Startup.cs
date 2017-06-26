using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OCTS.Startup))]
namespace OCTS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
