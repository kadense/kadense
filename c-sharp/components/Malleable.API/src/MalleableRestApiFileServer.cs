
namespace Kadense.Malleable.API
{
    public class MalleableRestApiFileServer : MalleableRestApiBase
    {
        public MalleableRestApiFileServer(IList<MalleableAssembly> assemblies, string? basePath = null, string prefix = "/api/namespaces") : base(assemblies, prefix)
        {
            BasePath = basePath ?? Environment.GetEnvironmentVariable("MFS_BASE_PATH")!;
        }

        public string BasePath { get; set; }

        protected override IEndpointRouteBuilder MapRoutes<T>(IEndpointRouteBuilder endpoints)
        {
            var path = CreatePath<T>();
            var directoryInfo = Directory.CreateDirectory(path);
            return base.MapRoutes<T>(endpoints);
        }

        protected string CreatePath<T>(string? identifier = null)
        {
            var attr = MalleableClassAttribute.FromType<T>();
            if(!string.IsNullOrEmpty(identifier))
                return $"{BasePath}/{attr.ModuleNamespace}/{attr.ModuleName}/{attr.ClassName}/{identifier}.json";

            else
                return $"{BasePath}/{attr.ModuleNamespace}/{attr.ModuleName}/{attr.ClassName}";
        }

        override protected async Task ProcessGetAsync<T>(HttpContext context, string identifier)
        {
            var path = CreatePath<T>(identifier);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            using(var stream = fileInfo.OpenRead())
            {
                context.Response.ContentLength = stream.Length;
                await stream.CopyToAsync(context.Response.Body);
                await context.Response.CompleteAsync();
                stream.Close();
            }
        }

        override protected async Task ProcessListAsync<T>(HttpContext context)
        {
            var path = CreatePath<T>();
            var folder = new DirectoryInfo(path);
            
            var items = new List<T>();
            var files = folder.GetFiles("*.json");
            foreach(var file in files)
            {
                using(var stream = file.OpenRead())
                {
                    var item = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, this.GetJsonSerializerOptions());
                    items.Add(item!);
                    stream.Close();
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(items, this.GetJsonSerializerOptions());
            await context.Response.CompleteAsync();
        }
        override protected async Task ProcessPutAsync<T>(HttpContext context, T content, string identifier)
        {
            var path = CreatePath<T>(identifier);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            fileInfo.Delete();
            using(var stream = fileInfo.Create())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync<T>(stream, content, this.GetJsonSerializerOptions());
                await context.Response.WriteAsJsonAsync<T>(content, this.GetJsonSerializerOptions());
                await context.Response.CompleteAsync();
                stream.Close();
            }
        }
        override protected async Task ProcessDeleteAsync<T>(HttpContext context, T content, string identifier)
        {
            var path = CreatePath<T>(identifier);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            using(var stream = fileInfo.OpenRead())
            {
                context.Response.ContentLength = stream.Length;
                await stream.CopyToAsync(context.Response.Body);
                await context.Response.CompleteAsync();
                stream.Close();
            }
            fileInfo.Delete();
        }
        protected override async Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            var path = CreatePath<T>(content.GetIdentifier());
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            using(var stream = fileInfo.Create())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync<T>(stream, content, this.GetJsonSerializerOptions());
                await context.Response.WriteAsJsonAsync<T>(content, this.GetJsonSerializerOptions());
                await context.Response.CompleteAsync();
                stream.Close();
            }
        }
    }
}