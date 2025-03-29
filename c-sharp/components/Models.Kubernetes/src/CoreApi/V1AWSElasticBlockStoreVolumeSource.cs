namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AWSElasticBlockStoreVolumeSource
    { 
        public string? FsType { get; set; }

        public string? VolumeID { get; set; }

        public int? Partition { get; set; }

        public bool? ReadOnly { get; set; }
    }
}