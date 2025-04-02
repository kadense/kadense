using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;

namespace Kadense.Jupyternetes.Operator.Tests {
    public class PodTests : KadenseTest
    {
        private JupyterNotebookInstance CreateInstance()
        {
            var variables = new Dictionary<string, string>();
            variables.Add("test", "test2");

            return new JupyterNotebookInstance()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
                    Name = "test-instance",
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookInstanceSpec()
                {
                    Template = new NotebookTemplate(){
                        Name = "test-template"
                    },
                    Variables = variables
                }
            };
        }

        private JupyterNotebookTemplate CreateTemplate()
        {
            return new JupyterNotebookTemplate()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
                    Name = "test-template",
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookTemplateSpec()
                {
                    Pods = new List<NotebookTemplatePodSpec>(){
                        new NotebookTemplatePodSpec(){
                            Name = "test-pod",
                            Labels = new Dictionary<string, string>()
                            {
                                { "jupyternetes.kadense.io/testProperty" , "{test}-instance" }
                            },
                            Spec = new V1PodSpec()
                            {
                                Containers = new List<V1Container>()
                                {
                                    new V1Container() {
                                        Name = "test-container",
                                        Image = "who-knows" 
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        [TestOrder(0)]
        [Fact]
        public  Task CreateJupyternetesInstance()
        {
            return Task.CompletedTask;
        }
    }
}