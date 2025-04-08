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
        public JupyterResourceState(string name, string? resourceName = null, string? state = null, string? errorMessage = null)
        {
            Name = name;
            ResourceName = resourceName;
            State = state;
            ErrorMessage = errorMessage;
        }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("resourceName")]
        public string? ResourceName { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}