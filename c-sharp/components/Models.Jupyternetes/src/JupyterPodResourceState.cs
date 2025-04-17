using YamlDotNet.Core.Tokens;

namespace Kadense.Models.Jupyternetes
{
    public class JupyterPodResourceState : JupyterResourceState
    {
        public JupyterPodResourceState()
        {

        }

        public JupyterPodResourceState(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        public JupyterPodResourceState(string? resourceName = null, string? state = null, string? errorMessage = null, string? podAddress = null, int? portNumber = null)
            : base(resourceName, state, errorMessage)
        {
            PodAddress = podAddress;
            PortNumber = portNumber;
        }

        [JsonPropertyName("podAddress")]
        public string? PodAddress { get; set; }

        [JsonPropertyName("portNumber")]
        public int? PortNumber { get; set; }
    }
}