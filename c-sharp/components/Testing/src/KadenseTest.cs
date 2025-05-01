using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Kadense.Testing
{
    [TestCaseOrderer(
        ordererTypeName: "Kadense.Testing.TestOrderer",
        ordererAssemblyName: "Kadense.Testing")]
    public class KadenseTest
    {
        public KadenseTest(ITestOutputHelper output)
        {
            this.Output = output;
        }

        public ITestOutputHelper Output { get; }
    }
}