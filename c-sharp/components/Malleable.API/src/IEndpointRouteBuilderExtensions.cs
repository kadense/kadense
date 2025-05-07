using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Kadense.Malleable.API
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapPost<T>(this IEndpointRouteBuilder endpoints, Delegate handler)
            where T : MalleableBase
        {
            return MapPost<T>(endpoints, handler, "/api/namespaces");
        }

        public static IEndpointRouteBuilder MapPost<T>(this IEndpointRouteBuilder endpoints, Delegate handler, string prefix = "/api/namespaces")
            where T : MalleableBase
        {
            var type = typeof(T);
            var malleableClassAttributes = type.GetCustomAttributes(typeof(MalleableClassAttribute), false);
            if (malleableClassAttributes.Length == 0)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have the MalleableClassAttribute.");
            }
            var malleableClassAttribute = (MalleableClassAttribute)malleableClassAttributes.First();
            endpoints.MapPost($"{prefix}/{malleableClassAttribute!.ModuleNamespace}/{malleableClassAttribute!.ModuleName}/{malleableClassAttribute.ClassName}", handler);
            return endpoints;
        }

        public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder endpoints, Type malleableType, Delegate handler)
        {
            typeof(IEndpointRouteBuilderExtensions)
                .GetMethod(nameof(MapPost), new Type[] { typeof(IEndpointRouteBuilder), typeof(Delegate) })
                ?.MakeGenericMethod(malleableType)
                .Invoke(null, new object[] { endpoints, handler });
            return endpoints;
        }

        public static IEndpointRouteBuilder MapPut<T>(this IEndpointRouteBuilder endpoints, Delegate handler, string prefix = "/api/namespaces")
            where T : MalleableBase
        {
            var type = typeof(T);
            var malleableClassAttributes = type.GetCustomAttributes(typeof(MalleableClassAttribute), false);
            if (malleableClassAttributes.Length == 0)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have the MalleableClassAttribute.");
            }
            var malleableClassAttribute = (MalleableClassAttribute)malleableClassAttributes.First();
            endpoints.MapPut($"{prefix}/{malleableClassAttribute!.ModuleNamespace}/{malleableClassAttribute!.ModuleName}/{malleableClassAttribute.ClassName}/{{identifier}}", handler);
            return endpoints;
        }

        public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder endpoints, Type malleableType, Delegate handler)
        {
            typeof(IEndpointRouteBuilderExtensions)
                .GetMethod(nameof(MapPut), new Type[] { typeof(IEndpointRouteBuilder), typeof(Delegate) })
                ?.MakeGenericMethod(malleableType)
                .Invoke(null, new object[] { endpoints, handler });
            return endpoints;
        }

        public static IEndpointRouteBuilder MapDelete<T>(this IEndpointRouteBuilder endpoints, Delegate handler, string prefix = "/api/namespaces")
            where T : MalleableBase
        {
            var type = typeof(T);
            var malleableClassAttributes = type.GetCustomAttributes(typeof(MalleableClassAttribute), false);
            if (malleableClassAttributes.Length == 0)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have the MalleableClassAttribute.");
            }
            var malleableClassAttribute = (MalleableClassAttribute)malleableClassAttributes.First();
            endpoints.MapDelete($"{prefix}/{malleableClassAttribute!.ModuleNamespace}/{malleableClassAttribute!.ModuleName}/{malleableClassAttribute.ClassName}/{{identifier}}", handler);
            return endpoints;
        }

        public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder endpoints, Type malleableType, Delegate handler)
        {
            typeof(IEndpointRouteBuilderExtensions)
                .GetMethod(nameof(MapDelete), new Type[] { typeof(IEndpointRouteBuilder), typeof(Delegate) })
                ?.MakeGenericMethod(malleableType)
                .Invoke(null, new object[] { endpoints, handler });
            return endpoints;
        }

        public static IEndpointRouteBuilder MapGet<T>(this IEndpointRouteBuilder endpoints, Delegate handler, string prefix = "/api/namespaces")
            where T : MalleableBase
        {
            var type = typeof(T);
            var malleableClassAttributes = type.GetCustomAttributes(typeof(MalleableClassAttribute), false);
            if (malleableClassAttributes.Length == 0)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have the MalleableClassAttribute.");
            }
            var malleableClassAttribute = (MalleableClassAttribute)malleableClassAttributes.First();
            endpoints.MapGet($"{prefix}/{malleableClassAttribute!.ModuleNamespace}/{malleableClassAttribute!.ModuleName}/{malleableClassAttribute.ClassName}/{{identifier}}", handler);
            return endpoints;
        }

        public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder endpoints, Type malleableType, Delegate handler)
        {
            typeof(IEndpointRouteBuilderExtensions)
                .GetMethod(nameof(MapGet), new Type[] { typeof(IEndpointRouteBuilder), typeof(Delegate) })
                ?.MakeGenericMethod(malleableType)
                .Invoke(null, new object[] { endpoints, handler });
            return endpoints;
        }

        public static IEndpointRouteBuilder MapList<T>(this IEndpointRouteBuilder endpoints, Delegate handler, string prefix = "/api/namespaces")
            where T : MalleableBase
        {
            var type = typeof(T);
            var malleableClassAttributes = type.GetCustomAttributes(typeof(MalleableClassAttribute), false);
            if (malleableClassAttributes.Length == 0)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have the MalleableClassAttribute.");
            }
            var malleableClassAttribute = (MalleableClassAttribute)malleableClassAttributes.First();
            endpoints.MapGet($"{prefix}/{malleableClassAttribute!.ModuleNamespace}/{malleableClassAttribute!.ModuleName}/{malleableClassAttribute.ClassName}", handler);
            return endpoints;
        }

        public static IEndpointRouteBuilder MapList(this IEndpointRouteBuilder endpoints, Type malleableType, Delegate handler)
        {
            typeof(IEndpointRouteBuilderExtensions)
                .GetMethod(nameof(MapList), new Type[] { typeof(IEndpointRouteBuilder), typeof(Delegate) })
                ?.MakeGenericMethod(malleableType)
                .Invoke(null, new object[] { endpoints, handler });
            return endpoints;
        }

        public static IEndpointRouteBuilder MapMalleableApi<T>(this IEndpointRouteBuilder endpoints, MalleableApiBase malleableApi)
            where T : MalleableBase
        {
            return malleableApi.Process<T>(endpoints);
        }

        public static IEndpointRouteBuilder MapMalleableApi(this IEndpointRouteBuilder endpoints, MalleableApiBase malleableApi, Type type)
        {
            return malleableApi.Process(endpoints, type);
        }
    }
}