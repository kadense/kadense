
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ScaleIOVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ScaleIOVolumeSource>
    {
        public string? FsType { get; set; }
        public string? Gateway { get; set; }
        public string? ProtectionDomain { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public bool? SslEnabled { get; set; }
        public string? StorageMode { get; set; }
        public string? StoragePool { get; set; }
        public string? System { get; set; }
        public string? VolumeName { get; set; }

        public override k8s.Models.V1ScaleIOVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ScaleIOVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                gateway: this.GetValue(this.Gateway, variables),
                protectionDomain: this.GetValue(this.ProtectionDomain, variables),
                readOnlyProperty: this.ReadOnly,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                sslEnabled: this.SslEnabled,
                storageMode: this.GetValue(this.StorageMode, variables),
                storagePool: this.GetValue(this.StoragePool, variables),
                system: this.GetValue(this.System, variables),
                volumeName: this.GetValue(this.VolumeName, variables)
            );
        }
    }
}