using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PassionProject1.Startup))]
namespace PassionProject1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
