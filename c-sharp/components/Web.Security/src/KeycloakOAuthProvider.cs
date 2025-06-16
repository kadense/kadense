using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadense.Web.Security
{
    public class KeycloakOAuthProvider : IOAuthProvider
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _realm;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public string Name => "Keycloak";

        public KeycloakOAuthProvider(
            string? baseUrl = null,
            string? realm = null,
            string? clientId = null,
            string? clientSecret = null,
            HttpClient? httpClient = null)
        {
            _baseUrl = (baseUrl ?? Environment.GetEnvironmentVariable("KEYCLOAK_BASE_URL") ?? "").TrimEnd('/');
            _realm = realm ?? Environment.GetEnvironmentVariable("KEYCLOAK_REALM") ?? "";
            _clientId = clientId ?? Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_ID") ?? "";
            _clientSecret = clientSecret ?? Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_SECRET") ?? "";
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var tokenEndpoint = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret)
            });

            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

            return new OAuthTokenResponse
            {
                AccessToken = data?["access_token"]?.ToString(),
                IdToken = data?["id_token"]?.ToString(),
                RefreshToken = data?["refresh_token"]?.ToString(),
                ExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToInt32(data?["expires_in"] ?? "3600")),
                Raw = data
            };
        }

        public async Task<OAuthUserInfo> GetUserInfoAsync(string accessToken)
        {
            var userInfoEndpoint = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/userinfo";
            var request = new HttpRequestMessage(HttpMethod.Get, userInfoEndpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            return new OAuthUserInfo
            {
                Subject = data?["sub"]?.ToString(),
                Email = data?["email"]?.ToString(),
                Claims = data
            };
        }
    }
}
