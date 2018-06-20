using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webform.Startup))]
namespace webform
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
