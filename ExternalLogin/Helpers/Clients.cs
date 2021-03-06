﻿using System;
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
                    AccessTokenType = AccessTokenType.Jwt,
                    Flow = Flows.Implicit,
                    RequireConsent = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("idsrv3test".Sha256())
                    },

                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44342/"
                    },
                    PostLogoutRedirectUris  = new List<string>
                    {
                        "https://localhost:44342/"
                    }
                }
           };
        }
    }
}