namespace Kadense.Malleable.Reflection;

public static class Extensions
{
    public static MalleableWorkflowApiFactory SetUnderlyingType<T>(this MalleableWorkflowApiFactory factory)
        where T : MalleableBase
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<MalleableClassAttribute>() ?? throw new InvalidOperationException($"Type {type.FullName} is not a Malleable class.");
        factory.Api.UnderlyingType = new MalleableTypeReference()
        {
            ClassName = attribute.ClassName,
            ModuleName = attribute.ModuleName,
            ModuleNamespace = attribute.ModuleNamespace
        };
        return factory;
    }
}