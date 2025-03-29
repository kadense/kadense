namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Volume
    {
        public V1AWSElasticBlockStoreVolumeSource? AWSElasticBlockStore { get; set; }
        public V1AzureDiskVolumeSource? AzureDisk { get; set; }
        public V1AzureFileVolumeSource? AzureFile { get; set; }
        public V1CephFSVolumeSource? CephFS { get; set; }
        public V1CinderVolumeSource? Cinder { get; set; }
        public V1ConfigMapVolumeSource? ConfigMap { get; set; }
        public V1DownwardAPIVolumeSource? DownwardAPI { get; set; }
        public V1EmptyDirVolumeSource? EmptyDir { get; set; }
        public V1EphemeralVolumeSource? Ephemeral { get; set; }
        public V1FCVolumeSource? Fc { get; set; }
        public V1FlexVolumeSource? FlexVolume { get; set; }
        public V1FlockerVolumeSource? Flocker { get; set; }
        public V1GCEPersistentDiskVolumeSource? GCEPersistentDisk { get; set; }
        public V1GlusterfsVolumeSource? Glusterfs { get; set; }
        public V1HostPathVolumeSource? HostPath { get; set; }
        public V1ImageVolumeSource? Image { get; set; }
        public V1ISCSIVolumeSource? ISCSI { get; set; }
        public string? Name { get; set; }
        public V1NFSVolumeSource? NFS { get; set; }
        public V1PersistentVolumeClaimVolumeSource? PersistentVolumeClaim { get; set; }
        public V1PhotonPersistentDiskVolumeSource? PhotonPersistentDisk { get; set; }
        public V1PortworxVolumeSource? PortworxVolume { get; set; }
        public V1ProjectedVolumeSource? Projected { get; set; }
        public V1QuobyteVolumeSource? Quobyte { get; set; }
        public V1RBDVolumeSource? RBD { get; set; }
        public V1ScaleIOVolumeSource? ScaleIO { get; set; }
        public V1SecretVolumeSource? Secret { get; set; }
        public V1StorageOSVolumeSource? StorageOS { get; set; }
        public V1VsphereVirtualDiskVolumeSource? VsphereVolume { get; set; }
    }
}