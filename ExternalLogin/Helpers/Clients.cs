using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace ExternalLogin.Helpers
{
    public static  class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "client name",
                    ClientId = "client1",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,

                    Flow = Flows.Implicit,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    },

                    AllowedScopes = new List<string>
                    {
                        "scope1"
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44342/"
                    }   
                }
           };
        }
    }
}