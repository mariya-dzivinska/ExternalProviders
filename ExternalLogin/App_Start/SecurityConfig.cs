using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using ExternalLogin.Helpers;
using IdentityServer3.Core.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Thinktecture.IdentityModel.Client;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;
using UserInfoClient = IdentityModel.Client.UserInfoClient;

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
                    Authority = "https://localhost:44342/identity/",
                    Scope = "openid profile email",
                    ClientId = "client1",
                    RedirectUri = "https://localhost:44342/",
                    ResponseType = "code",

                    SignInAsAuthenticationType = "Cookies",

                    Notifications =
                        new OpenIdConnectAuthenticationNotifications
                        {
                            AuthorizationCodeReceived = async n =>
                            {
                                var identity = n.AuthenticationTicket.Identity;

                                var nIdentity = new ClaimsIdentity(identity.AuthenticationType, "email", "role");

                                var userInfoClient = new UserInfoClient(
                                    new Uri("https://localhost:44342/identity/connect/userinfo"),
                                    n.ProtocolMessage.AccessToken);

                                var userInfo = await userInfoClient.GetAsync();
                                userInfo.Claims.ToList().ForEach(x => nIdentity.AddClaim(new Claim(x.Item1, x.Item2)));

                                var tokenClient = new OAuth2Client(new Uri("https://localhost:44342/identity/connect/userinfo/connect/token"), "client1", "idsrv3test");
                                var response = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);

                                nIdentity.AddClaim(new Claim("access_token", response.AccessToken));
                                nIdentity.AddClaim(
                                    new Claim("expires_at", DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToLocalTime().ToString(CultureInfo.InvariantCulture)));
                                nIdentity.AddClaim(new Claim("refresh_token", response.RefreshToken));
                                nIdentity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                                n.AuthenticationTicket = new AuthenticationTicket(
                                    nIdentity,
                                    n.AuthenticationTicket.Properties);
                            },
                            RedirectToIdentityProvider = async n =>
                            {
                                if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                                {
                                    var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                                    n.ProtocolMessage.IdTokenHint = idTokenHint;
                                }
                            }
                        }
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
                Caption = "Google login",
                SignInAsAuthenticationType = signInAsType,
                ClientId = clientId,
                ClientSecret = clientSecret,

            };
            app.UseGoogleAuthentication(options);
        }
    }
}