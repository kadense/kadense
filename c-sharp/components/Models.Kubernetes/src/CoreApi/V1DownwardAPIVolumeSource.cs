namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1DownwardAPIVolumeSource
    {
        public int? DefaultMode { get; set; }
        public List<V1DownwardAPIVolumeFile>? Items { get; set; }
    }
}