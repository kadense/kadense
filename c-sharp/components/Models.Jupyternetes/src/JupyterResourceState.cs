using YamlDotNet.Core.Tokens;

namespace Kadense.Models.Jupyternetes
{
    public class JupyterResourceState
    {
        public JupyterResourceState()
        {

        }

        public JupyterResourceState(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        public JupyterResourceState(string? resourceName = null, string? state = null, string? errorMessage = null)
        {
            ResourceName = resourceName;
            State = state;
            ErrorMessage = errorMessage;
        }

        [JsonPropertyName("resourceName")]
        public string? ResourceName { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("parameters")]
        public Dictionary<string, string>? Parameters { get; set; } = new Dictionary<string, string>();
    }
}