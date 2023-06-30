using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        #region Authorization Code Flow
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ClientCredentialsTokenRequest _tokenRequest;

        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
        //{
        //    _httpClientFactory = httpClientFactory;
        //    _tokenRequest = tokenRequest;
        //}
        #endregion

        #region Hybrid Flow
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            #region Authorization Code Flow
            //var httpClient = _httpClientFactory.CreateClient("IDPClient");

            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);
            //if (tokenResponse.IsError)
            //    throw new HttpRequestException("Something went wrong while requesting the access token");

            //request.SetBearerToken(tokenResponse.AccessToken);
            #endregion

            #region Hybrid Flow
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if(!string.IsNullOrWhiteSpace(accessToken))
                request.SetBearerToken(accessToken);
            #endregion

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
