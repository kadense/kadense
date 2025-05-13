using System.Text.Json;

namespace Kadense.Malleable.API
{
    public abstract class MalleableRestApiBase
    {
        public MalleableRestApiBase(IList<MalleableAssembly> assemblies, string prefix = "/api/namespaces") 
        {
            Assemblies = assemblies;
            Prefix = prefix;
        }

        public string Prefix { get; set; }


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
            where T : MalleableBase, IMalleableIdentifiable
        {
            return MapRoutes<T>(endpoints);
        }


        protected virtual IEndpointRouteBuilder MapRoutes<T>(IEndpointRouteBuilder endpoints)
            where T : MalleableBase, IMalleableIdentifiable        
        {
            return endpoints
            .MapGet<T>(async (HttpContext ctx, string identifier) =>
            {
                await ProcessGetAsync<T>(ctx, identifier);
            })
            .MapList<T>(async (HttpContext ctx) =>
            {
                await ProcessListAsync<T>(ctx);
            })   
            .MapPost<T>(async (HttpContext ctx) =>
            {
                var content = await ctx.Request.ReadFromJsonAsync<T>();
                await ProcessPostAsync<T>(ctx, content!);
            })
            .MapPut<T>(async (HttpContext ctx, string identifier) =>
            {
                var content = await ctx.Request.ReadFromJsonAsync<T>();
                await ProcessPutAsync<T>(ctx, content!, identifier);
            })
            .MapDelete<T>(async (HttpContext ctx, string identifier) =>
            {
                var content = await ctx.Request.ReadFromJsonAsync<T>();
                await ProcessDeleteAsync<T>(ctx, content!, identifier);
            });
            ;
        }

        protected abstract Task ProcessGetAsync<T>(HttpContext context, string identifier)
            where T : MalleableBase, IMalleableIdentifiable;
        protected abstract Task ProcessPostAsync<T>(HttpContext context, T content)
            where T : MalleableBase, IMalleableIdentifiable;
        protected abstract Task ProcessListAsync<T>(HttpContext context)
            where T : MalleableBase;
        protected abstract Task ProcessPutAsync<T>(HttpContext context, T content, string identifier)
            where T : MalleableBase, IMalleableIdentifiable;
        protected abstract Task ProcessDeleteAsync<T>(HttpContext context, T content, string identifier)
            where T : MalleableBase, IMalleableIdentifiable;

        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }
        public IList<MalleableAssembly> Assemblies { get; }
        public JsonSerializerOptions GetJsonSerializerOptions()
        {
            if(TypeResolver == null)
            {
                TypeResolver = new MalleablePolymorphicTypeResolver();
                foreach (var assembly in Assemblies)
                {
                    TypeResolver.MalleableAssembly.Add(assembly);
                }
            }
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = TypeResolver,
                WriteIndented = true
            };

            return options;
        }
    }
}