namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EnvFromSource
    {
        public V1ConfigMapEnvSource? ConfigMapRef { get; set; }
        public V1SecretEnvSource? SecretRef { get; set; }
        public string? Prefix { get; set; }
    }
}