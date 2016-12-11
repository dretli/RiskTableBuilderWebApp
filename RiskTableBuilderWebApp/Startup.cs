using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RiskTableBuilderWebApp.Startup))]
namespace RiskTableBuilderWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
