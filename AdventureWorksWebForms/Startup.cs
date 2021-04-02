using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdventureWorksWebForms.Startup))]
namespace AdventureWorksWebForms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
