using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using ExternalLogin.Helpers;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;

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
                    .UseInMemoryScopes(Scopes.Get()),
                AuthenticationOptions = new AuthenticationOptions
                {
                    EnablePostSignOutAutoRedirect = true,
                    IdentityProviders = ConfigureIdentityProviders
                }
            };

            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(serverOptions);
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = "sub";
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    Authority = "https://localhost:44385/identity",
                    Scope = "openid profile email",
                    ClientId = "client1",
                    RedirectUri = "https://localhost:44385/",
                    ResponseType = "id_token token",

                    SignInAsAuthenticationType = "Cookies"
                });
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
            var options = new GoogleOAuth2AuthenticationOptions
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