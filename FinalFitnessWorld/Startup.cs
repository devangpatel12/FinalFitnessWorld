using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalFitnessWorld.Startup))]
namespace FinalFitnessWorld
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
