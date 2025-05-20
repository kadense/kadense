using Kadense.Client.Kubernetes;
using Xunit.Abstractions;

namespace Kadense.Models.Malleable.Tests {
    public class MalleableModuleTests : KadenseTest
    {
        public MalleableModuleTests(ITestOutputHelper output) : base(output)
        {

        }

        public MalleableMockers Mockers { get; set; } = new MalleableMockers();

        [Fact]
        public void TestModuleCreation()
        {
            var module = Mockers.MockModule();
            Assert.NotNull(module);
            Assert.Equal("test-module", module.Metadata.Name);
            Assert.Equal("test-namespace", module.Metadata.NamespaceProperty);
            Assert.Contains("app", module.Metadata.Labels.Keys);
        }

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

        [Fact]
        public void TestFhirExample()
        {
            var module = Mockers.MockFhirModule();
            Assert.NotNull(module);
            Assert.Equal("fhir-test", module.Metadata.Name);
            Assert.Equal("default", module.Metadata.NamespaceProperty);
            Assert.Contains("Reference", module.Spec!.Classes!.Keys);
            var @class = module.Spec.Classes!["Reference"];
            Assert.Contains("reference", @class.Properties!.Keys);
            var property = @class.Properties!["reference"];
            Assert.Equal("string", property.Type);
        }

        [Fact]
        public async Task TestReadWriteDelete()
        {
            var server = new MalleableMockApi();
            server.Start(this.Output);
            var client = server.CreateClient();
            var crFactory = new CustomResourceClientFactory();
            var customResourceClient = crFactory.Create<MalleableModule>(client);

            var existingItems = await customResourceClient.ListNamespacedAsync("default");
            Assert.Single(existingItems.Items);

            var item = existingItems.Items.First();
            Assert.Equal("test-module", item.Metadata.Name);
            Assert.Equal("default", item.Metadata.NamespaceProperty);
            Assert.Contains("TestClass", item.Spec!.Classes!);
            var @class = item.Spec.Classes!["TestClass"];
            Assert.Contains("TestProperty", @class.Properties!);
            var property = @class.Properties!["TestProperty"];
            Assert.Equal("string", property.Type);

            var createdItem = await customResourceClient.CreateNamespacedAsync(item);
            var readItem = await customResourceClient.ReadNamespacedAsync(item.Metadata.NamespaceProperty, item.Metadata.Name);
            var updatedItem = await customResourceClient.ReplaceNamespacedAsync(item);
            var deletedItem = await customResourceClient.DeleteNamespacedAsync(item.Metadata.NamespaceProperty, item.Metadata.Name);

            Assert.Contains("TestClass2", createdItem.Spec!.Classes!);
            Assert.Contains("TestClass3", readItem.Spec!.Classes!);
        }

        [Fact]
        public void TestModuleSortAndCreate()
        {
            var mocker = new MalleableMockers();
            var modules = mocker.MockModules();
            Assert.NotNull(modules);
            modules.Sort();
            Assert.Equal(8, modules.Count);
            var last = modules.Last();
            Assert.Equal("composite-copc", last.Metadata.Name);
        }
    }
}