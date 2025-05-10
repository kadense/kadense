using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Kadense.Malleable.Reflection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MalleableConverterAttribute : Attribute
    {
        public MalleableConverterAttribute(string moduleNamespace, string moduleName, string converterName, string convertFromModuleNamespace, string convertFromModuleName, string convertFromClassName, string convertToModuleNamespace, string convertToModuleName, string convertToClassName)
        {
            ModuleNamespace = moduleNamespace;
            ModuleName = moduleName;
            ConverterName = converterName;
            ConvertFromModuleNamespace = convertFromModuleNamespace;
            ConvertFromModuleName = convertFromModuleName;
            ConvertFromClassName = convertFromClassName;
            ConvertToModuleNamespace = convertToModuleNamespace;
            ConvertToModuleName = convertToModuleName;
            ConvertToClassName = convertToClassName;
        }


        public string ConverterName { get; }
        public string ModuleNamespace { get; }
        public string ModuleName { get; }

        public string ConvertFromModuleNamespace { get; set; }
        public string ConvertFromModuleName { get; set; }
        public string ConvertFromClassName { get; set; }
        public string ConvertToModuleNamespace { get; set; }
        public string ConvertToModuleName { get; set; }
        public string ConvertToClassName { get; set; }

        public static MalleableConverterAttribute FromType(Type type)
        {
            return type.GetCustomAttribute<MalleableConverterAttribute>()!;
        }

        public static MalleableConverterAttribute FromType<T>()
        {
            var type = typeof(T);
            return FromType(type);
        }

        public string GetConvertFromModuleName()
        {
            return $"{ConvertFromModuleNamespace}:{ConvertFromModuleName}";
        }
        public string GetConvertToModuleName()
        {
            return $"{ConvertToModuleNamespace}:{ConvertToModuleName}";
        }
        
        public string GetQualifiedConverterName()
        {
            return $"{ModuleNamespace}:{ModuleName}:{ConverterName}";
        }
        
        public string GetModuleName()
        {
            return $"{ModuleNamespace}:{ModuleName}";
        }
    }
}