using IdentityServerHost.Quickstart.UI;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configure controllers and views for IdentityServer4 QuicStart UI
            builder.Services.AddControllersWithViews();

            // IdentityServer
            builder.Services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddTestUsers(TestUsers.Users) // changes from "Config.TestUsers" to "TestUsers.Users" for Claim based authorization in Hybrid flow
                .AddDeveloperSigningCredential();

            builder.Services.AddAuthentication("indentity_server_cookie_06282023")
                .AddCookie("indentity_server_cookie_06282023", options =>
                {
                    options.Cookie.Name = "indentity_server_cookie";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.SlidingExpiration = true;
                });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}