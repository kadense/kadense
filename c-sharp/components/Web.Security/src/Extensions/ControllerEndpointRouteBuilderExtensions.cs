namespace Kadense.Web.Security.Extensions
{
    public static class ControllerEndpointRouteBuilderExtensions
    {
        public static IMvcBuilder AddOpenIddictAuthorizationController(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(typeof(ControllerEndpointRouteBuilderExtensions).Assembly);
        }
    }
}