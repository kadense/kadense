using System.IO;
namespace Kadense.CustomResourceDefinition.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new instance of the CustomResourceDefinitionFactory

            string outputPath = "./";

            if(args.Count() > 0)
            {
                outputPath = args[0];
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) // Use camel case for property names
                .IgnoreFields()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull) // Omit null values
                .Build();

            // Create a CRD for JupyterNotebookTemplate and JupyterNotebookInstance
            var notebookTemplateCRD = SerializeToYaml<JupyterNotebookTemplate>(serializer, outputPath);
            var notebookInstanceCRD = SerializeToYaml<JupyterNotebookInstance>(serializer, outputPath);
            var malleableModuleCRD = SerializeToYaml<MalleableModule>(serializer, outputPath);
            var malleableConverterModuleCRD = SerializeToYaml<MalleableConverterModule>(serializer, outputPath);

            // Output the CRDs to the console or use them as needed
            
        }

        static string SerializeToYaml<T>(ISerializer serializer)
        {
            var crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<T>();
            crd.ApiVersion = "apiextensions.k8s.io/v1";
            crd.Kind = "CustomResourceDefinition";
            return serializer.Serialize(crd);
        }

        static string SerializeToYaml<T>(ISerializer serializer, string outputPath)
        {
            var type = typeof(T);
            var yaml = SerializeToYaml<T>(serializer);
            string path = $"{outputPath}/{type.Name}-CRD.yaml";
            using(var writer = new StreamWriter(path))
            {
                writer.Write(yaml);
                writer.Close();
            }
            Console.WriteLine($"Serialized {type.Name} to {path}");
            return path;
        }


    }
}

