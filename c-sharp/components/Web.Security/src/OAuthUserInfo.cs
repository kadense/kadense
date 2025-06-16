using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kadense.Web.Security
{
    /// <summary>
    /// Represents user information retrieved from an OAuth provider.
    /// </summary>
    public class OAuthUserInfo
    {
        /// <summary>
        /// The unique subject identifier for the user.
        /// </summary>
        [JsonPropertyName("sub")]
        public string? Subject { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Additional claims associated with the user.
        /// </summary>
        [JsonPropertyName("claims")]
        public IDictionary<string, object>? Claims { get; set; }
    }
}
