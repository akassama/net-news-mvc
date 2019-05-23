using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GambiaReview.Startup))]
namespace GambiaReview
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
