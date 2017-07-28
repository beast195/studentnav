using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentNav.Startup))]
namespace StudentNav
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
