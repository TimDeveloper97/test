using NTS.Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLTK.Business.InitData;
using QLTK.Business.AutoMappers;

//[assembly: OwinStartup(typeof(QLTK.Api.Startup))]
[assembly: OwinStartupAttribute("QLTK", typeof(QLTK.Api.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace QLTK.Api
{
    /// <summary>
    /// This class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up.
    /// The “Configuration” method accepts parameter of type “IAppBuilder” this parameter will be supplied by the host at run-time.
    /// This “app” parameter is an interface which will be used to compose the application for our Owin server.
    /// The “HttpConfiguration” object is used to configure API routes, so we’ll pass this object to method “Register” in “WebApiConfig” class.
    /// </summary>
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            log4net.Config.XmlConfigurator.Configure();

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR();

            AutoMapperConfig.Init();
            InitPermission.Init();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //allow both http and https
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                Provider = new SimpleAuthorizationServerProvider(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}