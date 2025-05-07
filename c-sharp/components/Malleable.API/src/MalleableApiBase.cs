namespace Kadense.Malleable.API
{
    public abstract class MalleableApiBase
    {

        public MalleableApiBase(string prefix = "/api/namespaces")
        {
            Prefix = prefix;
        }

        public virtual IEndpointRouteBuilder Process(IEndpointRouteBuilder endpoints, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                this.Process(endpoints, type);
            }

            return endpoints;
        }

        public virtual IEndpointRouteBuilder Process(IEndpointRouteBuilder endpoints, Type type)
        {
            var method = this.GetType().GetMethod(nameof(MapRoutes), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.MakeGenericMethod(new Type[] { type });
            method.Invoke(this, new object[] { endpoints });
            return endpoints;
        }


        public virtual IEndpointRouteBuilder Process<T>(IEndpointRouteBuilder endpoints)
            where T : MalleableBase
        {
            return MapRoutes<T>(endpoints);
        }

        protected virtual IEndpointRouteBuilder MapRoutes<T>(IEndpointRouteBuilder endpoints)
            where T : MalleableBase
        {

            return endpoints.MapPost<T>(async (HttpContext ctx) =>
            {
                var content = await ctx.Request.ReadFromJsonAsync<T>();
                await ProcessPostAsync<T>(ctx, content!);
            });
        }

        protected abstract Task ProcessPostAsync<T>(HttpContext context, T content)
            where T : MalleableBase;

        public string Prefix { get; set; }
    }
}