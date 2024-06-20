using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Vista_Administrador.StartupOwin))]

namespace Vista_Administrador
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
