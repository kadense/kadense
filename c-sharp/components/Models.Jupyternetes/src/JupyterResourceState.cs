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
        public JupyterResourceState(string name, string resourceName, string state)
        {
            name = name;
            ResourceName = resourceName;
            State = state;
        }

        public string? Name { get; set; }
        public string? ResourceName { get; set; }
        public string? State { get; set; }
        public string? ErrorMessage { get; set; }
    }
}