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
using Syncfusion.Licensing;

[assembly: OwinStartup(typeof(NTSFile.API.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace NTSFile.API
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHFqVVhkW1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jTH9TdkNgWH5cd3VVQA==;Mgo+DSMBPh8sVXJ0S0d+XE9AcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xTc0RjWXldcXVXQWNUVw==;ORg4AjUWIQA/Gnt2VVhhQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkBiWX9Yc3VURWBdUEc=;NjM0MzIwQDMyMzAyZTMxMmUzMGFGQnR1YTB1N3lNNUhnczIyZEs3b0JVT3IxNkh1QWhKaEpvR2JZQTErZU09;NjM0MzIxQDMyMzAyZTMxMmUzMFg5dDNMVnRPZ2VDNlhNMnBzN25LdGJaK1p6aVJ2UE5tNXdOWUZVVjl2MUE9;NRAiBiAaIQQuGjN/V0Z+XU9EaFtFVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdEVmWH9edndUR2NfUEN1;NjM0MzIzQDMyMzAyZTMxMmUzMGN5SkhJaXVzOTVWeW8yQUtLY2NmelY4cGNrWGVxdC9GUVRKRVZuTmU3bzQ9;NjM0MzI0QDMyMzAyZTMxMmUzMEF1bWg4eFJWbWFOR1VUbDYrQTRpdVVWUU1vY0xUVXdZUVRJelhzekl3RjQ9;Mgo+DSMBMAY9C3t2VVhhQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkBiWX9Yc3VURWRfV0U=;NjM0MzI2QDMyMzAyZTMxMmUzMFJjKzREUHlLc3g2amkwdmlvN25IMENpT0F0Y1o3SE1rejVhMkJ4b3FSUms9;NjM0MzI3QDMyMzAyZTMxMmUzME5iMXRRMDRwWE04a0Mrc056TVo0dlo3eUxSakpwenFtQk5ocFc0ZEhVN1E9");

            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            log4net.Config.XmlConfigurator.Configure();

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
            };

            // Token Generation
        }
    }
}