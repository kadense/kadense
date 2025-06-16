using System;
using System.Threading.Tasks;

namespace Kadense.Web.Security
{
    public class DummyOAuthProvider : IOAuthProvider
    {
        private readonly string _expectedUserId;
        private readonly string _expectedPassword;

        public DummyOAuthProvider()
        {
            _expectedUserId = Environment.GetEnvironmentVariable("DUMMY_OAUTH_USERID") ?? "test.user@kadense.io";
            _expectedPassword = Environment.GetEnvironmentVariable("DUMMY_OAUTH_PASSWORD") ?? "kadense";
        }

        public string Name => "Dummy";

        public Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            // In this dummy provider, treat 'code' as the password and 'redirectUri' as the user id.
            if (code == _expectedPassword && redirectUri == _expectedUserId)
            {
                var token = Guid.NewGuid().ToString();
                return Task.FromResult(new OAuthTokenResponse
                {
                    AccessToken = token,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                });
            }
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        public Task<OAuthUserInfo> GetUserInfoAsync(string accessToken)
        {
            // Always return the dummy user if the access token is not empty.
            if (!string.IsNullOrEmpty(accessToken))
            {
                return Task.FromResult(new OAuthUserInfo
                {
                    Subject = Guid.NewGuid().ToString(),
                    Email = _expectedUserId,
                    Claims = new System.Collections.Generic.Dictionary<string, object>
                    {

                    }
                });
            }
            throw new UnauthorizedAccessException("Invalid access token.");
        }
    }
}
