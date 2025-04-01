
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeDevice : KadenseTemplatedCopiedResource<k8s.Models.V1VolumeDevice>
    {
        public string? Name { get; set; }
        public string? DevicePath { get; set; }

        public override k8s.Models.V1VolumeDevice ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1VolumeDevice(
                name: this.GetValue(this.Name, variables),
                devicePath: this.GetValue(this.DevicePath, variables)
            );
        }
    }
}