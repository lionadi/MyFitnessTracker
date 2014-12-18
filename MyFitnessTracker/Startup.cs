using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyFitnessTracker.Startup))]
namespace MyFitnessTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
