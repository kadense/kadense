using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kadense.Web.Security
{
    /// <summary>
    /// Provides methods for handling OAuth authentication flows and user information retrieval.
    /// </summary>
    public class OAuthService
    {
        private readonly IOAuthProvider _provider;
        private readonly IClaimsTransformer _claimsTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthService"/> class.
        /// </summary>
        /// <param name="provider">The OAuth provider.</param>
        /// <param name="claimsTransformer">The claims transformer.</param>
        public OAuthService(IOAuthProvider provider, IClaimsTransformer claimsTransformer)
        {
            _provider = provider;
            _claimsTransformer = claimsTransformer;
        }

        /// <summary>
        /// Authenticates a user using the provided authorization code and redirect URI.
        /// </summary>
        /// <param name="code">The authorization code received from the OAuth provider.</param>
        /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the authenticated principal.</returns>
        public async Task<ClaimsPrincipal> AuthenticateAsync(string code, string redirectUri)
        {
            // Exchange code for tokens
            var tokenResponse = await _provider.ExchangeCodeAsync(code, redirectUri);

            // Get user info from provider
            var userInfo = await _provider.GetUserInfoAsync(tokenResponse.AccessToken!);

            // Create initial claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Subject!),
                new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)
            };

            // Add provider claims
            foreach (var kv in userInfo.Claims!)
            {
                claims.Add(new Claim(kv.Key, kv.Value?.ToString() ?? string.Empty));
            }

            // Transform/add custom claims
            var finalClaims = _claimsTransformer.TransformClaims(userInfo, claims);

            var identity = new ClaimsIdentity(finalClaims, _provider.Name);
            return new ClaimsPrincipal(identity);
        }
    }
}
