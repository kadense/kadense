namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookInstanceStatus
    {
        [JsonPropertyName("pvcs")]
        public List<JupyterResourceState> Pvcs { get; set; } = new List<JupyterResourceState>();

        [JsonPropertyName("pods")]
        public List<JupyterResourceState> Pods { get; set; } = new List<JupyterResourceState>();

        [JsonPropertyName("podsProvisioned")]
        public string PodsProvisioningState { get; set; } = "Pending";

        [JsonPropertyName("pvcsProvisioned")]
        public string PvcsProvisionedState { get; set; } = "Pending";
    }
}