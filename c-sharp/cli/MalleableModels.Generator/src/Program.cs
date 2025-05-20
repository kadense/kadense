using System.IO;
using Kadense.Malleable.Workflow.NHS.Extensions;

namespace Kadense.MalleableModels.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new instance of the factory

            string outputPath = "./";

            if(args.Count() > 0)
            {
                outputPath = args[0];
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) // Use camel case for property names
                .IgnoreFields()
                .WithQuotingNecessaryStrings(true)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull) // Omit null values
                .Build();

            var factory = new MalleableModuleFactory();
            // Create a Resource for the MalleableModel's

            var fhirSTU3 = factory.FromFhirSchema("kadense", "fhir-stu3");
            var fhirSTU3Modules = factory.SplitModule(fhirSTU3);
            foreach(var module in fhirSTU3Modules)
            {
                SerializeToYaml(module, serializer, outputPath);
            }
        }

        
        static string SerializeToYaml(MalleableModule model, ISerializer serializer, string outputPath)
        {
            var yaml = serializer.Serialize(model);
            string path = $"{outputPath}/{model.Metadata.NamespaceProperty}-{model.Metadata.Name}-Model.yaml";
            using(var writer = new StreamWriter(path))
            {
                writer.Write(yaml);
                writer.Close();
            }
            Console.WriteLine($"Serialized {model.Metadata.NamespaceProperty}/{model.Metadata.Name} to {path}");
            return path;
        }
    }
}

