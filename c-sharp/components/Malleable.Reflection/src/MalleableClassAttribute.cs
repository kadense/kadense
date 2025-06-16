using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Kadense.Malleable.Reflection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MalleableClassAttribute : Attribute
    {
        public MalleableClassAttribute(string moduleNamespace, string moduleName, string className)
        {
            ModuleNamespace = moduleNamespace;
            ModuleName = moduleName;
            ClassName = className;
        }

        public string ClassName { get; }
        public string ModuleNamespace { get; }
        public string ModuleName { get; }

        public static MalleableClassAttribute FromType(Type type)
        {
            return type.GetCustomAttribute<MalleableClassAttribute>()!;
        }

        public static MalleableClassAttribute FromType<T>()
        {
            var type = typeof(T);
            return FromType(type);
        }

        public string GetQualifiedModuleName()
        {
            return $"{ModuleNamespace}:{ModuleName}";
        }

        public string GetQualifiedClassName()
        {
            return $"{GetQualifiedModuleName()}:{ClassName}";
        }
    }
}