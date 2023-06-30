using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Data;

namespace Movies.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MoviesAPIContext>(options =>
                options.UseInMemoryDatabase("Movies"));

            // activate JWT Authentication
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5005";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                    };
                });

            // activate Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MovieClient06282023Policy", policy => policy.RequireClaim("client_id", "movieClient", "moviesMVClient")); // added also 'moviesMVClient' for Hybrid Flow
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // seed database
            SeedDatabase(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void SeedDatabase(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var services = scope.ServiceProvider;
            var moviesContext = services.GetRequiredService<MoviesAPIContext>();
            MoviesContextSeed.SeedAsync(moviesContext);
        }
    }
}