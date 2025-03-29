namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeMount
    {
        public string? Name { get; set; }
        public string? MountPath { get; set; }
        public bool? ReadOnly { get; set; }
        public string? MountPropagation { get; set; }
        public string? SubPath { get; set; }
        public string? SubPathExpr { get; set; }
        public string? RecursiveReadOnly { get; set; }
    }
}