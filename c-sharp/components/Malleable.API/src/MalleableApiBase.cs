namespace Kadense.Malleable.API
{
    public abstract class MalleableApiBase
    {

        public MalleableApiBase(string prefix = "/api/namespaces")
        {
            Prefix = prefix;
        }

        public virtual IEndpointRouteBuilder Process(IEndpointRouteBuilder endpoints, IEnumerable<Type> types, string prefix = "/api/namespaces", string? suffix = null)
        {
            foreach (var type in types)
            {
                this.Process(endpoints, type, prefix, suffix);
            }

            return endpoints;
        }

        public virtual IEndpointRouteBuilder Process(IEndpointRouteBuilder endpoints, Type type, string prefix = "/api/namespaces", string? suffix = null)
        {
            var method = this.GetType().GetMethod(nameof(MapRoutes), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.MakeGenericMethod(new Type[] { type });
            method.Invoke(this, new object?[] { endpoints, prefix, suffix });
            return endpoints;
        }



        protected virtual IEndpointRouteBuilder MapRoutes<T>(IEndpointRouteBuilder endpoints, string prefix = "/api/namespaces", string? suffix = null)
            where T : MalleableBase
        {

            return endpoints.MapPost<T>(async (HttpContext ctx) =>
            {
                var content = await ctx.Request.ReadFromJsonAsync<T>();
                await ProcessPostAsync<T>(ctx, content!);
            }, prefix, suffix);
        }

        protected abstract Task ProcessPostAsync<T>(HttpContext context, T content)
            where T : MalleableBase;

        public string Prefix { get; set; }
    }
}