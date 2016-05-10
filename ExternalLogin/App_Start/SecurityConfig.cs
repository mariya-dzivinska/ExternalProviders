using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using ExternalLogin.Helpers;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace ExternalLogin
{
    public static class SecurityConfig
    {
        public static void Configure(IAppBuilder app)
        {
            var serverOptions = new IdentityServerOptions
            {
                SiteName = "Identity server 3",
                SigningCertificate = LoadCertificate(),

                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(Users.Get())
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(StandardScopes.All),
                AuthenticationOptions = new AuthenticationOptions
                {
                    IdentityProviders = ConfigureIdentityProviders
                }
            };

            app.UseIdentityServer(serverOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseExternalSignInCookie("Cookies");

        }

        private static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\identityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory),
                "idsrv3test");
        }

        public static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var clientId = ConfigurationManager.AppSettings["GoogleClientId"];
            var clientSecret = ConfigurationManager.AppSettings["GoogleSecret"];
            var options = new GoogleOAuth2AuthenticationOptions()
            {
                AuthenticationType = "Google",
                Caption = "Google",
                SignInAsAuthenticationType = signInAsType,
                ClientId = clientId,
                ClientSecret = clientSecret,

            };
            app.UseGoogleAuthentication(options);
        }
    }
}