using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Net.Sockets;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    // RedirectUris = { urlClient },
                    // PostLogoutRedirectUris = { urlClient },
                    // AllowedCorsOrigins = { urlClient },
                    AbsoluteRefreshTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    SlidingRefreshTokenLifetime = 60,
                    AlwaysSendClientClaims = true,
                    ClientSecrets =
                    {
                        new Secret("movie06282023APIsecret".Sha256())
                    },
                    AllowedScopes = {"movieAPI"},
                    AccessTokenLifetime = 60,
                    IdentityTokenLifetime = 60,
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("movieAPI", "Movie API")
            };
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { };
        public static List<TestUser> TestUsers => new List<TestUser> { };
    }
}
