namespace Kadense.Models.Kubernetes
{
    // Attribute to tell Kadense to allow any value for this property
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class KubernetesAllowAnyValueAttribute : Attribute
    {

    }
}