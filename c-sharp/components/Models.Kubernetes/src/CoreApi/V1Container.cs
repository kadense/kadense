namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Container
    {
        public List<string>? Args { get; set; }
        public List<string>? Command { get; set; }
        public V1EnvVar? Env { get; set; }
        public List<V1EnvFromSource>? EnvFrom { get; set; }
        public string? Image { get; set; }
        public string? ImagePullPolicy { get; set; }
        public V1Lifecycle? Lifecycle { get; set; }
        public V1Probe? LivenessProbe { get; set; }
        public string? Name { get; set; }
        public V1ContainerPort? Ports { get; set; }
        public V1Probe? ReadinessProbe { get; set; }
        public List<V1ContainerResizePolicy>? ResizePolicy { get; set; }
        public V1ResourceRequirements? Resources { get; set; }
        public string? RestartPolicy { get; set; }
        public V1SecurityContext? SecurityContext { get; set; }
        public V1Probe? StartupProbe { get; set; }
        public bool? Stdin { get; set; }
        public bool? StdinOnce { get; set; }
        public string? TerminationMessagePath { get; set; }
        public string? TerminationMessagePolicy { get; set; }
        public bool? Tty { get; set; }
        public List<V1VolumeDevice>? VolumeDevices { get; set; }
        public List<V1VolumeMount>? VolumeMounts { get; set; }
        public string? WorkingDir { get; set; }
    }
}