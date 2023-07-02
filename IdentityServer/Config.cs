using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

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
                    ClientSecrets =
                    {
                        new Secret("movie06282023APIsecret".Sha256())
                    },
                    AllowedScopes = {"movieAPI"},
                    AccessTokenLifetime = 60,
                    IdentityTokenLifetime = 60,
                },
                new Client
                {
                    ClientId = "moviesMVClient",
                    ClientName = "Movies MVC App",
                    AccessTokenType = AccessTokenType.Jwt,
                    AbsoluteRefreshTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    SlidingRefreshTokenLifetime = 60,
                    AccessTokenLifetime = 60,
                    IdentityTokenLifetime = 60,
                    AllowedGrantTypes = GrantTypes.Hybrid, // change from Code to Hybrid
                    RequirePkce = false, // additional config for Hybrid flow
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("movie06282023MVCsecret".Sha256())
                    },
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address, // added for Claim based authorization in Hybrid flow
                        IdentityServerConstants.StandardScopes.Email, // added for Claim based authorization in Hybrid flow
                        "movieAPI", // also added the scope for Movie API to consume the same token with the Movie MVC Client when login via OpenID Connect
                        "roles", // added for Claim based authorization in Hybrid flow
                    }
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("movieAPI", "Movie API")
            };
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(), // added for Claim based authorization in Hybrid flow
            new IdentityResources.Email(), // added for Claim based authorization in Hybrid flow
            new IdentityResource("roles", "Your role(s)", new List<string>(){"role"}) // added for Claim based authorization in Hybrid flow
        };
        public static List<TestUser> TestUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "MM3242QQ-435M-TM25-MT4Q-50NM2905U2N3",
                Username = "mahdiguliyev",
                Password = "Mahdi135@",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "mahdi"),
                    new Claim(JwtClaimTypes.FamilyName, "guliyev"),
                }
            }
        };
    }
}
