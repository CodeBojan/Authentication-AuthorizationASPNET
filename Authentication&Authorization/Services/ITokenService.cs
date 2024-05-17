using IdentityModel.Client;

namespace Authentication_Authorization.Services
{
    public interface ITokenService
    {
        public Task<TokenResponse> GetTokenAsync(string scope);
    }
}
