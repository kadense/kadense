namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Volume : KadenseTemplatedCopiedResource<k8s.Models.V1Volume>
    {
        [JsonPropertyName("awsElasticBlockStore")]
        public V1AWSElasticBlockStoreVolumeSource? AWSElasticBlockStore { get; set; }

        [JsonPropertyName("azureDisk")]
        public V1AzureDiskVolumeSource? AzureDisk { get; set; }

        [JsonPropertyName("azureFile")]
        public V1AzureFileVolumeSource? AzureFile { get; set; }

        [JsonPropertyName("cephfs")]
        public V1CephFSVolumeSource? CephFS { get; set; }

        [JsonPropertyName("cinder")]
        public V1CinderVolumeSource? Cinder { get; set; }

        [JsonPropertyName("configMap")]
        public V1ConfigMapVolumeSource? ConfigMap { get; set; }

        [JsonPropertyName("downwardAPI")]
        public V1DownwardAPIVolumeSource? DownwardAPI { get; set; }

        [JsonPropertyName("emptyDir")]
        public V1EmptyDirVolumeSource? EmptyDir { get; set; }

        [JsonPropertyName("ephemeral")]
        public V1EphemeralVolumeSource? Ephemeral { get; set; }

        [JsonPropertyName("fc")]
        public V1FCVolumeSource? Fc { get; set; }

        [JsonPropertyName("flexVolume")]
        public V1FlexVolumeSource? FlexVolume { get; set; }

        [JsonPropertyName("flocker")]
        public V1FlockerVolumeSource? Flocker { get; set; }

        [JsonPropertyName("gcePersistentDisk")]
        public V1GCEPersistentDiskVolumeSource? GCEPersistentDisk { get; set; }

        [JsonPropertyName("glusterfs")]
        public V1GlusterfsVolumeSource? Glusterfs { get; set; }

        [JsonPropertyName("hostPath")]
        public V1HostPathVolumeSource? HostPath { get; set; }

        [JsonPropertyName("image")]
        public V1ImageVolumeSource? Image { get; set; }

        [JsonPropertyName("iscsi")]
        public V1ISCSIVolumeSource? ISCSI { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("nfs")]
        public V1NFSVolumeSource? NFS { get; set; }

        [JsonPropertyName("persistentVolumeClaim")]
        public V1PersistentVolumeClaimVolumeSource? PersistentVolumeClaim { get; set; }

        [JsonPropertyName("photonPersistentDisk")]
        public V1PhotonPersistentDiskVolumeSource? PhotonPersistentDisk { get; set; }

        [JsonPropertyName("portworxVolume")]
        public V1PortworxVolumeSource? PortworxVolume { get; set; }

        [JsonPropertyName("projected")]
        public V1ProjectedVolumeSource? Projected { get; set; }

        [JsonPropertyName("quobyte")]
        public V1QuobyteVolumeSource? Quobyte { get; set; }

        [JsonPropertyName("rbd")]
        public V1RBDVolumeSource? RBD { get; set; }

        [JsonPropertyName("scaleIO")]
        public V1ScaleIOVolumeSource? ScaleIO { get; set; }

        [JsonPropertyName("secret")]
        public V1SecretVolumeSource? Secret { get; set; }

        [JsonPropertyName("storageos")]
        public V1StorageOSVolumeSource? StorageOS { get; set; }

        [JsonPropertyName("vsphereVolume")]
        public V1VsphereVirtualDiskVolumeSource? VsphereVolume { get; set; }

        public override k8s.Models.V1Volume ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Volume(
                name: this.GetValue(this.Name, variables),
                awsElasticBlockStore: this.AWSElasticBlockStore != null ? this.AWSElasticBlockStore.ToOriginal(variables) : null,
                azureDisk: this.AzureDisk != null ? this.AzureDisk.ToOriginal(variables) : null,
                azureFile: this.AzureFile != null ? this.AzureFile.ToOriginal(variables) : null,
                cephfs: this.CephFS != null ? this.CephFS.ToOriginal(variables) : null,
                cinder: this.Cinder != null ? this.Cinder.ToOriginal(variables) : null,
                configMap: this.ConfigMap != null ? this.ConfigMap.ToOriginal(variables) : null,
                downwardAPI: this.DownwardAPI != null ? this.DownwardAPI.ToOriginal(variables) : null,
                emptyDir: this.EmptyDir != null ? this.EmptyDir.ToOriginal(variables) : null,
                ephemeral: this.Ephemeral != null ? this.Ephemeral.ToOriginal(variables) : null,
                fc: this.Fc != null ? this.Fc.ToOriginal(variables) : null,
                flexVolume: this.FlexVolume != null ? this.FlexVolume.ToOriginal(variables) : null,
                flocker: this.Flocker != null ? this.Flocker.ToOriginal(variables) : null,
                gcePersistentDisk: this.GCEPersistentDisk != null ? this.GCEPersistentDisk.ToOriginal(variables) : null,
                glusterfs: this.Glusterfs != null ? this.Glusterfs.ToOriginal(variables) : null,
                hostPath: this.HostPath != null ? this.HostPath.ToOriginal(variables) : null,
                image: this.Image != null ? this.Image.ToOriginal(variables) : null,
                iscsi: this.ISCSI != null ? this.ISCSI.ToOriginal(variables) : null,
                nfs: this.NFS != null ? this.NFS.ToOriginal(variables) : null,
                persistentVolumeClaim: this.PersistentVolumeClaim != null ? this.PersistentVolumeClaim.ToOriginal(variables) : null,
                photonPersistentDisk: this.PhotonPersistentDisk != null ? this.PhotonPersistentDisk.ToOriginal(variables) : null,
                portworxVolume: this.PortworxVolume != null ? this.PortworxVolume.ToOriginal(variables) : null,
                projected: this.Projected != null ? this.Projected.ToOriginal(variables) : null,
                quobyte: this.Quobyte != null ? this.Quobyte.ToOriginal(variables) : null,
                rbd: this.RBD != null ? this.RBD.ToOriginal(variables) : null,
                scaleIO: this.ScaleIO != null ? this.ScaleIO.ToOriginal(variables) : null,
                secret: this.Secret != null ? this.Secret.ToOriginal(variables) : null,
                storageos: this.StorageOS != null ? this.StorageOS.ToOriginal(variables) : null,
                vsphereVolume: this.VsphereVolume != null ? this.VsphereVolume.ToOriginal(variables) : null
            );
        }
    }
}