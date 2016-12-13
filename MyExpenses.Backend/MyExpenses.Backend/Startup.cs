using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyExpenses.Backend.Startup))]

namespace MyExpenses.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}