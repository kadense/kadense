using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Web.Security
{
    /// <summary>
    /// Provides extension methods for mapping OAuth-related endpoints in an ASP.NET Core application.
    /// </summary>
    public static class OAuthEndpointExtensions
    {
        public static IEndpointRouteBuilder MapKadenseOAuthToKeycloakProvider(
            this IEndpointRouteBuilder endpoints,
            IClaimsTransformer? claimsTransformer = null,
            string loginPath = "/oauth/login",
            string callbackPath = "/oauth/callback",
            string userInfoPath = "/oauth/userinfo")
        {
            var oauthService = new OAuthService(
                new KeycloakOAuthProvider(),
                claimsTransformer ?? new BasicClaimsTransformer()
            );
            return MapKadenseOAuth(endpoints, oauthService, loginPath, callbackPath, userInfoPath);
        }
        public static IEndpointRouteBuilder MapKadenseOAuthToDummyProvider(
            this IEndpointRouteBuilder endpoints,
            IClaimsTransformer? claimsTransformer = null,
            string loginPath = "/oauth/login",
            string callbackPath = "/oauth/callback",
            string userInfoPath = "/oauth/userinfo")
        {
            var oauthService = new OAuthService(
                new KeycloakOAuthProvider(),
                claimsTransformer ?? new BasicClaimsTransformer()
            );
            return MapKadenseOAuth(endpoints, oauthService, loginPath, callbackPath, userInfoPath);
        }
      
        /// <summary>
        /// Maps Kadense OAuth endpoints for login, callback, and user info.
        /// </summary>
        /// <param name="endpoints">The endpoint route builder to add routes to.</param>
        /// <param name="oauthService">The OAuth service used for authentication.</param>
        /// <param name="loginPath">The path for the login endpoint. Defaults to "/oauth/login".</param>
        /// <param name="callbackPath">The path for the callback endpoint. Defaults to "/oauth/callback".</param>
        /// <param name="userInfoPath">The path for the user info endpoint. Defaults to "/oauth/userinfo".</param>
        /// <returns>The endpoint route builder with the OAuth endpoints mapped.</returns>
        public static IEndpointRouteBuilder MapKadenseOAuth(
            this IEndpointRouteBuilder endpoints,
            OAuthService oauthService,
            string loginPath = "/oauth/login",
            string callbackPath = "/oauth/callback",
            string userInfoPath = "/oauth/userinfo")
        {
            endpoints.MapGet(loginPath, async context =>
            {
                // Redirect to external provider's authorization endpoint
                // (This is a placeholder; actual implementation should build the URL)
                var redirectUrl = context.Request.Query["redirect_uri"];
                // TODO: Build provider-specific authorization URL
                await context.Response.WriteAsync("Redirect to provider's authorization endpoint here.");
            });

            endpoints.MapGet(callbackPath, async context =>
            {
                var code = context.Request.Query["code"];
                var redirectUri = context.Request.Query["redirect_uri"];
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(redirectUri))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Missing code or redirect_uri.");
                    return;
                }

                var principal = await oauthService.AuthenticateAsync(code!, redirectUri!);
                // TODO: Issue cookie or token, or return claims as needed
                await context.Response.WriteAsync("Authenticated. Claims: " + string.Join(", ", principal.Claims));
            });

            endpoints.MapGet(userInfoPath, async context =>
            {
                // TODO: Extract user info from context.User or token
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    var claims = context.User.Claims;
                    await context.Response.WriteAsync("User info: " + string.Join(", ", claims));
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Not authenticated.");
                }
            });

            return endpoints;
        }
    }
}
