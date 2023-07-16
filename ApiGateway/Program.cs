
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add authetication for Ocelot Api Gateway
            var authenticationProviderKey = "IdentityApiKey";

            builder.Services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, auth =>
                {
                    auth.Authority = "https://localhost:5005"; // Identity Server URL
                    //auth.RequireHttpsMetadata = false;
                    auth.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                });

            // configure Ocelot services
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration);

            // Add services to the container.
            //builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.MapControllers();

            await app.UseOcelot();

            app.Run();
        }
    }
}