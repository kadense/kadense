using System.Reflection;
using Xunit;

namespace Kadense.Testing
{
    [TestCaseOrderer(
        ordererTypeName: "Kadense.Testing.TestOrderer",
        ordererAssemblyName: "Kadense.Testing")]
    public class KadenseTest
    {
        protected string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Resource '{resourceName}' not found.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}