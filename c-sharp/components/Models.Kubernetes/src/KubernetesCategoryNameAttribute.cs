using System;

namespace Kadense.Models.Kubernetes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class KubernetesCategoryNameAttribute : Attribute
    {
        public string Name { get; }

        public KubernetesCategoryNameAttribute(string name)
        {
            Name = name;
        }
    }
}