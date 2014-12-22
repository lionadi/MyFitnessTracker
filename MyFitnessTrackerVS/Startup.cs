using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyFitnessTrackerVS.Startup))]
namespace MyFitnessTrackerVS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
