using System.Reflection;
using Xunit;

namespace Kadense.Testing
{
    public class KadenseTestUtils
    {
        public static string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            try{
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        throw new InvalidOperationException($"Resource '{resourceName}' not found in assembly {assembly.GetName()}.");
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                var availableResources = string.Join(", ", assembly.GetManifestResourceNames());
                throw new InvalidOperationException($"Error reading resource '{resourceName}' in assembly {assembly.GetName()}.: available names: {availableResources}", ex);
            }
        }
    }
}