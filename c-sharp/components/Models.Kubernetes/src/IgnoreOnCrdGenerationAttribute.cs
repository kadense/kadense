namespace Kadense.Models.Kubernetes
{
    // Attribute to tell Kadense to ignore this on CRD Generation
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class IgnoreOnCrdGenerationAttribute : Attribute
    {

    }
}