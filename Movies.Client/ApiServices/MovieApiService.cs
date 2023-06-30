using IdentityModel.Client;
using Movies.Client.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/movies");
            request.Content = JsonContent.Create(movie);

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var contect = await response.Content.ReadAsStringAsync();
            var movieRes = JsonConvert.DeserializeObject<Movie>(contect);

            return movieRes;
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/movies/{id}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var contect = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(contect);

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            #region WAY 1

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var contect = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(contect);

            return movieList;
            #endregion

            #region WAY 2
            //// 1 - Get Token from Identity Server, of course we should provide the IS configurations like address, clientId and clientSecret.
            //// 2 - Send Request to protected API
            //// 3 - Deserialize object to MovieList

            //// 1.1. "retrieve" our api credentials. This must be registered on Identity Server!
            //var apiClientCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "movie06282023APIsecret",
            //    Scope = "movieAPI"
            //};

            //// creates a new HttpClient to talk to our IdentityServer
            //var client = new HttpClient();

            //// 1.2. Authenticates and get an access token from Identity Server
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            //if (tokenResponse.IsError)
            //    return null;

            //// 2.1. Send request to protected API
            //var apiClient = new HttpClient();

            //// set the access_token in the request Authorization: Bearer <token>
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            //// send the request to protected API
            //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();

            //// 3.1 Deserialize object to MovieList
            //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList;
            #endregion
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
