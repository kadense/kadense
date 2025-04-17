namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookInstanceStatus
    {
        [JsonPropertyName("pvcs")]
        public Dictionary<string, JupyterResourceState> Pvcs { get; set; } = new Dictionary<string, JupyterResourceState>();

        [JsonPropertyName("pods")]
        public Dictionary<string, JupyterPodResourceState> Pods { get; set; } = new Dictionary<string, JupyterPodResourceState>();

        [JsonPropertyName("otherResources")]
        public Dictionary<string, JupyterResourceState> OtherResources { get; set; } = new Dictionary<string,JupyterResourceState>();

        [JsonPropertyName("podsProvisioned")]
        public string PodsProvisionedState { get; set; } = "Pending";

        [JsonPropertyName("pvcsProvisioned")]
        public string PvcsProvisionedState { get; set; } = "Pending";

        [JsonPropertyName("otherResourcesProvisioned")]
        public Dictionary<string, bool> OtherResourcesProvisionedState { get; set; } = new Dictionary<string, bool>();
    }
}