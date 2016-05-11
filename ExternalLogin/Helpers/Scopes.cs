using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace ExternalLogin.Helpers
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "scope1",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("one")
                    }
                }
            };

                scopes.AddRange(StandardScopes.All);

                return scopes;
            }
    }
}