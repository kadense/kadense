namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EphemeralContainer : KadenseTemplatedCopiedResource<k8s.Models.V1EphemeralContainer>
    {
        public List<string>? Args { get; set; }
        public List<string>? Command { get; set; }
        public List<V1EnvVar>? Env { get; set; }
        public List<V1EnvFromSource>? EnvFrom { get; set; }
        public string? Image { get; set; }
        public string? ImagePullPolicy { get; set; }
        public string? Name { get; set; }
        public V1Lifecycle? Lifecycle { get; set; }
        public V1Probe? LivenessProbe { get; set; }
        public List<V1ContainerPort>? Ports { get; set; }
        public V1Probe? ReadinessProbe { get; set; }
        public List<V1ContainerResizePolicy>? ResizePolicy { get; set; }
        public V1ResourceRequirements? Resources { get; set; }
        public V1SecurityContext? SecurityContext { get; set; }
        public V1Probe? StartupProbe { get; set; }
        public string? RestartPolicy { get; set; }
        public bool? Stdin { get; set; }
        public bool? StdinOnce { get; set; }
        public string? TargetContainerName { get; set; }
        public string? TerminationMessagePath { get; set; }
        public string? TerminationMessagePolicy { get; set; }
        public bool? Tty { get; set; }
        public List<V1VolumeDevice>? VolumeDevices { get; set; }
        public List<V1VolumeMount>? VolumeMounts { get; set; }
        public string? WorkingDir { get; set; }

        public override k8s.Models.V1EphemeralContainer ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EphemeralContainer(
                name: this.GetValue(this.Name, variables),
                args: this.GetValue(this.Args, variables),
                command: this.GetValue(this.Command, variables),
                env: this.GetValue<V1EnvVar, k8s.Models.V1EnvVar>(this.Env, variables),
                envFrom: this.GetValue<V1EnvFromSource, k8s.Models.V1EnvFromSource>(this.EnvFrom, variables),
                image: this.GetValue(this.Image, variables),
                imagePullPolicy: this.GetValue(this.ImagePullPolicy, variables),
                lifecycle: this.Lifecycle != null ? this.Lifecycle.ToOriginal(variables) : null,
                livenessProbe: this.LivenessProbe != null ? this.LivenessProbe.ToOriginal(variables) : null,
                ports: this.GetValue<V1ContainerPort, k8s.Models.V1ContainerPort>(this.Ports, variables),
                readinessProbe: this.ReadinessProbe != null ? this.ReadinessProbe.ToOriginal(variables) : null,
                resizePolicy: this.GetValue<V1ContainerResizePolicy, k8s.Models.V1ContainerResizePolicy>(this.ResizePolicy, variables),
                resources: this.Resources != null ? this.Resources.ToOriginal(variables) : null,
                restartPolicy: this.GetValue(this.RestartPolicy, variables),
                securityContext: this.SecurityContext != null ? this.SecurityContext.ToOriginal(variables) : null,
                startupProbe: this.StartupProbe != null ? this.StartupProbe.ToOriginal(variables) : null,
                stdin: this.Stdin,
                stdinOnce: this.StdinOnce,
                terminationMessagePath: this.GetValue(this.TerminationMessagePath, variables),
                terminationMessagePolicy: this.GetValue(this.TerminationMessagePolicy, variables),
                tty: this.Tty,
                volumeDevices: this.GetValue<V1VolumeDevice, k8s.Models.V1VolumeDevice>(this.VolumeDevices, variables),
                volumeMounts: this.GetValue<V1VolumeMount, k8s.Models.V1VolumeMount>(this.VolumeMounts, variables),
                workingDir: this.GetValue(this.WorkingDir, variables)
            );
        }
    }
}