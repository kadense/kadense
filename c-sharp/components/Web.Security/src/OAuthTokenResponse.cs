using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kadense.Web.Security
{
    /// <summary>
    /// Represents the response containing OAuth tokens.
    /// </summary>
    public class OAuthTokenResponse
    {
        /// <summary>
        /// The type of the token, typically "Bearer".
        /// </summary>
        public string? TokenType { get; set; } = "Bearer";

        /// <summary>
        /// The OAuth access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        /// <summary>
        /// The OAuth ID token.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        /// <summary>
        /// The OAuth refresh token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        /// <summary>
        /// The expiration date and time of the access token.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// The raw token response as a dictionary.
        /// </summary>
        [JsonPropertyName("raw")]
        public IDictionary<string, object>? Raw { get; set; }
    }
}
