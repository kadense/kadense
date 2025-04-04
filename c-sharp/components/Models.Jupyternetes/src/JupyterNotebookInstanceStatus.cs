namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookInstanceStatus
    {
        [JsonPropertyName("pvcs")]
        public Dictionary<string, string> Pvcs { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("pods")]
        public Dictionary<string, string> Pods { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("podsProvisioned")]
        public string PodsProvisioningState { get; set; } = "Pending";

        [JsonPropertyName("pvcsProvisioned")]
        public string PvcsProvisionedState { get; set; } = "Pending";
    }
}