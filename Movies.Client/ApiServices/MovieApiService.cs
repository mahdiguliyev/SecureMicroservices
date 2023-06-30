using IdentityModel.Client;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            // 1 - Get Token from Identity Server, of course we should provide the IS configurations like address, clientId and clientSecret.
            // 2 - Send Request to protected API
            // 3 - Deserialize object to MovieList

            // 1.1. "retrieve" our api credentials. This must be registered on Identity Server!
            var apiClientCredentials = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5005/connect/token",
                ClientId = "movieClient",
                ClientSecret = "movie06282023APIsecret",
                Scope = "movieAPI"
            };

            // creates a new HttpClient to talk to our IdentityServer
            var client = new HttpClient();

            // 1.2. Authenticates and get an access token from Identity Server
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            if (tokenResponse.IsError)
                return null;

            // 2.1. Send request to protected API
            var apiClient = new HttpClient();
            
            // set the access_token in the request Authorization: Bearer <token>
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            // send the request to protected API
            var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // 3.1 Deserialize object to MovieList
            List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
