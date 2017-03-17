using System;
using System.Configuration;
using Abp.Hangfire;
using Abp.Owin;
using SuperRocket.Orchard.Api.Controllers;
using SuperRocket.Orchard.Web;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Hangfire.SqlServer;

[assembly: OwinStartup(typeof(Startup))]

namespace SuperRocket.Orchard.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();
           
            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
           
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            if (IsTrue("ExternalAuth.Facebook.IsEnabled"))
            {
                app.UseFacebookAuthentication(CreateFacebookAuthOptions());
            }

            if (IsTrue("ExternalAuth.Twitter.IsEnabled"))
            {
                app.UseTwitterAuthentication(CreateTwitterAuthOptions());
            }

            if (IsTrue("ExternalAuth.Google.IsEnabled"))
            {
                app.UseGoogleAuthentication(CreateGoogleAuthOptions());
            }

            app.MapSignalR();

            //ENABLE TO USE HANGFIRE dashboard (Requires enabling Hangfire in OrchardWebModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter() } //You can remove this line to disable authorization
            //});
            app.UseErrorPage();
            app.UseWelcomePage("/welcome");

            //GlobalConfiguration.Configuration.UseSqlServerStorage(
            //    "Default",
            //    new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });


            app.UseHangfireDashboard();
            // app.UseHangfireServer();

            //RecurringJob.AddOrUpdate(
            //    () => LongRunningConsole(),
            //    Cron.Minutely);
        }

        //public void LongRunningConsole()
        //{
        //    Console.WriteLine("{0} Recurring job completed successfully!", DateTime.Now.ToString());
        //}

        private static FacebookAuthenticationOptions CreateFacebookAuthOptions()
        {
            var options = new FacebookAuthenticationOptions
            {
                AppId = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppId"],
                AppSecret = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppSecret"]
            };

            options.Scope.Add("email");
            options.Scope.Add("public_profile");

            return options;
        }

        private static TwitterAuthenticationOptions CreateTwitterAuthOptions()
        {
            return new TwitterAuthenticationOptions
            {
                ConsumerKey = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerSecret"]
            };
        }

        private static GoogleOAuth2AuthenticationOptions CreateGoogleAuthOptions()
        {
            return new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientSecret"]
            };
        }

        private static bool IsTrue(string appSettingName)
        {
            return string.Equals(
                ConfigurationManager.AppSettings[appSettingName],
                "true",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}