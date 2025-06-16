using System.Threading.Tasks;

namespace Kadense.Web.Security
{
    /// <summary>
    /// Represents an OAuth provider capable of exchanging authorization codes for tokens and retrieving user information.
    /// </summary>
    public interface IOAuthProvider
    {
        /// <summary>
        /// Exchanges the specified authorization code for an access token.
        /// </summary>
        /// <param name="code">The authorization code received from the OAuth provider.</param>
        /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token response.</returns>
        Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri);

        /// <summary>
        /// Retrieves user information using the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token issued by the OAuth provider.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user information.</returns>
        Task<OAuthUserInfo> GetUserInfoAsync(string accessToken);

        /// <summary>
        /// Gets the name of the OAuth provider.
        /// </summary>
        string Name { get; }
    }
}
