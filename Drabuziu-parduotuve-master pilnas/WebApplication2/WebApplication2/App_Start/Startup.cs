using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(WebApplication2.App_Start.Startup))] // Update namespace to include App_Start

namespace WebApplication2.App_Start
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder  app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Authentication/Login"),
            });
        }
    }
}

