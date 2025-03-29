namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1DownwardAPIVolumeFile
    {
        public string? Path { get; set; }
        public V1ObjectFieldSelector? FieldRef { get; set; }
        public V1ResourceFieldSelector? ResourceFieldRef { get; set; }
    }
}