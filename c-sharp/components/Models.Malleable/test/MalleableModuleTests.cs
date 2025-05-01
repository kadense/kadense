using Kadense.Client.Kubernetes;

namespace Kadense.Models.Malleable.Tests {
    public class MalleableModuleTests
    {
        [Fact]
        public void TestCrdGeneration()
        {
            var crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<MalleableModule>();
            Assert.NotNull(crd);
            Assert.Equal("MalleableModule", crd.Spec.Names.Kind);
            Assert.Equal("malleablemodule", crd.Spec.Names.Singular);
            Assert.Equal("malleablemodules", crd.Spec.Names.Plural);
        }
    }
}