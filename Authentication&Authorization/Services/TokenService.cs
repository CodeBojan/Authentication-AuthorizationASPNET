using Authentication_Authorization.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Authentication_Authorization.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IOptions<IdentityServerSettings> _identityServerSettings;
        private readonly DiscoveryDocumentResponse _discoveryDocument;

        public TokenService(ILogger<TokenService> logger, IOptions<IdentityServerSettings> identityServerSettings) 
        {
            _logger = logger;
            _identityServerSettings = identityServerSettings;

            using var client = new HttpClient();
            _discoveryDocument = client.GetDiscoveryDocumentAsync(_identityServerSettings.Value.DiscoveryUrl).Result;
            if(_discoveryDocument.IsError)
            {
                _logger.LogError($"Unable to get discovery document at {_identityServerSettings.Value.DiscoveryUrl}");
                throw new Exception("Unable to get discovery document");
            }
        }
        public async Task<TokenResponse> GetTokenAsync(string scope)
        {
            using var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Scope = scope,
                ClientId = _identityServerSettings.Value.ClientName,
                ClientSecret = _identityServerSettings.Value.ClientSecret,
                Address = _discoveryDocument.TokenEndpoint
            });
            if(tokenResponse.IsError)
            {
                throw new Exception("Failed to get token");
            }

            return tokenResponse;
        }
    }
}
