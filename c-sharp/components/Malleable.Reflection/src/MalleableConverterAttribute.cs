using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Kadense.Malleable.Reflection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MalleableConverterAttribute : Attribute
    {
        public MalleableConverterAttribute(string moduleNamespace, string moduleName, string converterName)
        {
            ModuleNamespace = moduleNamespace;
            ModuleName = moduleName;
            ConverterName = converterName;
        }


        public string ConverterName { get; }
        public string ModuleNamespace { get; }
        public string ModuleName { get; }
        
        public string GetQualifiedConverterName()
        {
            return $"{ModuleNamespace}:{ModuleName}:{ConverterName}";
        }
        
        public string GetModuleName()
        {
            return $"{ModuleNamespace}:{ModuleName}";
        }

        public static MalleableConverterAttribute FromType(Type type)
        {
            return type.GetCustomAttribute<MalleableConverterAttribute>()!;
        }

        public static MalleableConverterAttribute FromType<T>()
        {
            var type = typeof(T);
            return FromType(type);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MalleableConvertFromAttribute : Attribute
    {
        public MalleableConvertFromAttribute(string moduleNamespace, string moduleName, string className)
        {
            ConvertFromModuleNamespace = moduleNamespace;
            ConvertFromModuleName = moduleName;
            ConvertFromClassName = className;
        }

        public string ConvertFromModuleNamespace { get; set; }
        public string ConvertFromModuleName { get; set; }
        public string ConvertFromClassName { get; set; }
        
        public string GetConvertFromModuleName()
        {
            return $"{ConvertFromModuleNamespace}:{ConvertFromModuleName}";
        }

        public static MalleableConvertFromAttribute FromType(Type type)
        {
            return type.GetCustomAttribute<MalleableConvertFromAttribute>()!;
        }

        public static MalleableConvertFromAttribute FromType<T>()
        {
            var type = typeof(T);
            return FromType(type);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MalleableConvertToAttribute : Attribute
    {
        public MalleableConvertToAttribute(string moduleNamespace, string moduleName, string className)
        {
            ConvertToModuleNamespace = moduleNamespace;
            ConvertToModuleName = moduleName;
            ConvertToClassName = className;
        }
        public string ConvertToModuleNamespace { get; set; }
        public string ConvertToModuleName { get; set; }
        public string ConvertToClassName { get; set; }

        public static MalleableConvertToAttribute FromType(Type type)
        {
            return type.GetCustomAttribute<MalleableConvertToAttribute>()!;
        }

        public static MalleableConvertToAttribute FromType<T>()
        {
            var type = typeof(T);
            return FromType(type);
        }
        public string GetConvertToModuleName()
        {
            return $"{ConvertToModuleNamespace}:{ConvertToModuleName}";
        }
    }
}