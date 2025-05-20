using Kadense.Malleable.API;
using Microsoft.AspNetCore.Http;

namespace Kadense.Malleable.Storage
{
    public class MalleableActorStorageProvider : MalleableRestApiBase
    {
        public MalleableActorStorageProvider(IList<MalleableAssembly> assemblies, IActorContext actorContext, string prefix) : base(assemblies, prefix)
        {
            
        }

        protected override Task ProcessDeleteAsync<T>(HttpContext context, T content, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override Task ProcessGetAsync<T>(HttpContext context, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override Task ProcessListAsync<T>(HttpContext context)
        {
            throw new NotImplementedException();
        }

        protected override Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            throw new NotImplementedException();
        }

        protected override Task ProcessPutAsync<T>(HttpContext context, T content, string identifier)
        {
            throw new NotImplementedException();
        }
    }
}