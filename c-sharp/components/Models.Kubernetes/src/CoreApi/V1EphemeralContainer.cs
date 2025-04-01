namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EphemeralContainer : KadenseTemplatedCopiedResource<k8s.Models.V1EphemeralContainer>
    {
        [JsonPropertyName("args")]
        public List<string>? Args { get; set; }

        [JsonPropertyName("command")]
        public List<string>? Command { get; set; }

        [JsonPropertyName("env")]
        public List<V1EnvVar>? Env { get; set; }

        [JsonPropertyName("envFrom")]
        public List<V1EnvFromSource>? EnvFrom { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("imagePullPolicy")]
        public string? ImagePullPolicy { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("lifecycle")]
        public V1Lifecycle? Lifecycle { get; set; }

        [JsonPropertyName("livenessProbe")]
        public V1Probe? LivenessProbe { get; set; }

        [JsonPropertyName("ports")]
        public List<V1ContainerPort>? Ports { get; set; }

        [JsonPropertyName("readinessProbe")]
        public V1Probe? ReadinessProbe { get; set; }

        [JsonPropertyName("resizePolicy")]
        public List<V1ContainerResizePolicy>? ResizePolicy { get; set; }

        [JsonPropertyName("resources")]
        public V1ResourceRequirements? Resources { get; set; }

        [JsonPropertyName("securityContext")]
        public V1SecurityContext? SecurityContext { get; set; }

        [JsonPropertyName("startupProbe")]
        public V1Probe? StartupProbe { get; set; }

        [JsonPropertyName("restartPolicy")]
        public string? RestartPolicy { get; set; }

        [JsonPropertyName("stdin")]
        public bool? Stdin { get; set; }

        [JsonPropertyName("stdinOnce")]
        public bool? StdinOnce { get; set; }

        [JsonPropertyName("targetContainerName")]
        public string? TargetContainerName { get; set; }

        [JsonPropertyName("terminationMessagePath")]
        public string? TerminationMessagePath { get; set; }

        [JsonPropertyName("terminationMessagePolicy")]
        public string? TerminationMessagePolicy { get; set; }

        [JsonPropertyName("tty")]
        public bool? Tty { get; set; }

        [JsonPropertyName("volumeDevices")]
        public List<V1VolumeDevice>? VolumeDevices { get; set; }

        [JsonPropertyName("volumeMounts")]
        public List<V1VolumeMount>? VolumeMounts { get; set; }

        [JsonPropertyName("workingDir")]
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