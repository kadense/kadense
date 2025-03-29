namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1FCVolumeSource
    {
        public string? FsType { get; set; }
        public int? Lun { get; set; }
        public bool? ReadOnly { get; set; }
        public List<string>? TargetWWNS { get; set; }
        public List<string>? Wwids { get; set; }
    }
}