using System;

namespace Kadense.Models.Kubernetes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class KubernetesShortNameAttribute : Attribute
    {
        public string Name { get; }

        public KubernetesShortNameAttribute(string name)
        {
            Name = name;
        }
    }
}