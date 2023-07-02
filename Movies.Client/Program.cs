using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;

namespace Movies.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // configure services with DI
            builder.Services.AddScoped<IMovieApiService, MovieApiService>();

            // authentication with OpenID Connect
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "movie_client_cookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5005";
                options.ClientId = "moviesMVClient";
                options.ClientSecret = "movie06282023MVCsecret";
                options.ResponseType = "code id_token"; // changed from 'code' to 'code id_token'
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");

                options.Scope.Add("address"); // added for Claim based authorization in Hybrid flow
                options.Scope.Add("email"); // added for Claim based authorization in Hybrid flow

                options.Scope.Add("movieAPI"); // added additional scope for the Movie API. When the Movie MVC Client login via OpenID Connect,
                                               // then Movie API also can use the same toke with Movie MVC Client

                options.Scope.Add("roles"); // added for Claim based authorization in Hybrid flow
                options.ClaimActions.MapUniqueJsonKey("role", "role"); // added for Claim based authorization in Hybrid flow

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.TokenValidationParameters = new TokenValidationParameters // added for Claim based authorization in Hybrid flow
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });

            // create a HttpClient for IHttpClientFactory to consume Movie API
            builder.Services.AddTransient<AuthenticationDelegatingHandler>();

            builder.Services.AddHttpClient("MovieAPIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            // create a HttpClient used for accessing the IDP
            builder.Services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5005/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            // For Hybrid flow below configuration should be added. We will get token via HttpContextAccess, when user login
            builder.Services.AddHttpContextAccessor();

            // ClientCredentialsTokenRequest is for Authorization Code Flow

            //builder.Services.AddSingleton(new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "movie06282023APIsecret",
            //    Scope = "movieAPI"
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}