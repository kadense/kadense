namespace Kadense.Malleable.API
{
    public class MalleableIngressApiFileServer : MalleableApiBase
    {
        public MalleableIngressApiFileServer(string? basePath = null, string prefix = "/api/namespaces") : base(prefix)
        {
            BasePath = basePath ?? Environment.GetEnvironmentVariable("MFS_BASE_PATH")!;
        }

        public string BasePath { get; set; }

        protected string CreatePath<T>(string? identifier = null)
        {
            var attr = MalleableClassAttribute.FromType<T>();
            if(!string.IsNullOrEmpty(identifier))
                return $"{BasePath}/{attr.ModuleNamespace}/{attr.ModuleName}/{attr.ClassName}/{identifier}.json";

            else
                return $"{BasePath}/{attr.ModuleNamespace}/{attr.ModuleName}/{attr.ClassName}";
        }
        
        protected override IEndpointRouteBuilder MapRoutes<T>(IEndpointRouteBuilder endpoints)
        {
            var path = CreatePath<T>();
            var directoryInfo = Directory.CreateDirectory(path);
            return base.MapRoutes<T>(endpoints);
        }

        protected override async Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            var identifier = Guid.NewGuid().ToString();
            var path = CreatePath<T>(identifier);
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Headers.Append("X-Identifier", identifier);
            using(var stream = fileInfo.Create())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync<T>(stream, content);
                await context.Response.WriteAsJsonAsync<T>(content);
                await context.Response.CompleteAsync();
                stream.Close();
            }
        }
    }
}