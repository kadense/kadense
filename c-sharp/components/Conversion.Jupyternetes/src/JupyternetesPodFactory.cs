using k8s.Models;
using Kadense.Models.Jupyternetes;

namespace Kadense.Conversion.Jupyternetes
{
    public class JupyternetesPodFactory
    {
        public JupyterNotebookTemplate Template { get; private set; }
        public JupyternetesPodFactory(JupyterNotebookTemplate template)
        {
            this.Template = template;
        }

    }
}