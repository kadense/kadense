using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Kadense.Malleable.Reflection
{
    public class MalleableDynamicLinqCustomTypeProviders : DefaultDynamicLinqCustomTypeProvider
    {
        public MalleableDynamicLinqCustomTypeProviders(ParsingConfig config, bool cacheCustomTypes = true) : base(config, cacheCustomTypes)
        {
        }

        public MalleableDynamicLinqCustomTypeProviders(ParsingConfig config, IList<Type> additionalTypes, bool cacheCustomTypes = true) : base(config, additionalTypes, cacheCustomTypes)
        {
        }
    }

}