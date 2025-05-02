using YamlDotNet.RepresentationModel;
using Xunit.Abstractions;
using Kadense.Client.Kubernetes;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Reflection.Tests {
    public class MalleableAssemblyFactoryTest : KadenseTest
    {
        public MalleableAssemblyFactoryTest(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void TestBuild()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            Assert.NotNull(malleableAssembly);
            Assert.NotEmpty(malleableAssembly.Types);
        }

        [Fact]
        public void TestInitialise()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var type = malleableAssembly.Types["TestClass"];
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
        }

        [Fact]
        public void TestInitialiseInheritedClass()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var type = malleableAssembly.Types["TestInheritedClass"];
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
        }

        [Fact]
        public void TestInitialiseAndConvert()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var type = malleableAssembly.Types["TestInheritedClass"];
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
            instance.GetType().GetProperty("TestString")!.SetValue(instance, "test1");
            var converter = mocker.MockConverterModule();
            var converterAssembly = malleableAssemblyFactory.CreateAssembly(
                converter,
                new Dictionary<string, MalleableAssembly>
                {
                    { $"{malleableModule.Metadata.NamespaceProperty}:{malleableModule.Metadata.Name}", malleableAssembly }
                });
            Assert.Contains("TestInheritedClass", converterAssembly.Types);
            Assert.Contains("ConvertedClass", converterAssembly.Types);
            Assert.Contains("Converter", converterAssembly.Types);

            var converterType = converterAssembly.Types["Converter"];
            var testInheritedClassType = converterAssembly.Types["TestInheritedClass"];
            var convertedClassType = converterAssembly.Types["ConvertedClass"];

            var converterInstance = Activator.CreateInstance(converterType);
            var method = converterType.GetMethod("ConvertFromTestInheritedClassToTestClass")!;
            
            var convertedInstance = method.Invoke(converterInstance, new object[] { instance });
            Assert.NotNull(convertedInstance);
            var testString = convertedClassType.GetProperty("TestStringV1")!.GetValue(convertedInstance);
            var testStringPrefix = convertedClassType.GetProperty("TestStringPrefix")!.GetValue(convertedInstance);
            Assert.Equal("test1", testString);
            Assert.Equal("t", testStringPrefix);
        }

        [Fact]
        public void TestAssignProperties()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var inheritingType = malleableAssembly.Types["TestInheritedClass"];
            var referencingType = malleableAssembly.Types["TestReferenceClass"];
            var inheritedInstance = Activator.CreateInstance(inheritingType);
            Assert.NotNull(inheritedInstance);

            inheritingType.GetProperty("TestString")!.SetValue(inheritedInstance, "test1");
            inheritingType.GetProperty("TestList")!.SetValue(inheritedInstance, new List<string> { "test2", "test3" });

            var testString = inheritingType.GetProperty("TestString")!.GetValue(inheritedInstance)!;
            var testList = (List<string>)inheritingType.GetProperty("TestList")!.GetValue(inheritedInstance)!;

            Assert.Equal("test1", testString);
            Assert.Equal(2, testList.Count);
            Assert.Equal("test2", testList[0]);
            Assert.Equal("test3", testList[1]);

            var referenceInstance = Activator.CreateInstance(referencingType);
            Assert.NotNull(referenceInstance);
            referencingType.GetProperty("TestReference")!.SetValue(referenceInstance, inheritedInstance);
            
            var referencedPropertyValue = referencingType.GetProperty("TestReference")!.GetValue(referenceInstance);
            Assert.NotNull(referencedPropertyValue);

            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), inheritingType);
            var dictionaryInstance = Activator.CreateInstance(dictionaryType)!;
            dictionaryInstance.GetType()!.GetMethod("Add")!.Invoke(dictionaryInstance, new object[] { "test", inheritedInstance });
            
            referencingType.GetProperty("DictionaryReference")!.SetValue(referenceInstance, dictionaryInstance);
            var dictionaryReferenceValue = referencingType.GetProperty("DictionaryReference")!.GetValue(referenceInstance);
            Assert.NotNull(dictionaryReferenceValue);
        }

        [Fact]
        public async Task TestK8sApi()
        {
            var server = new MalleableMockApi();
            server.Start(this.Output);
            var client = server.CreateClient();
            var crFactory = new CustomResourceClientFactory();
            var customResourceClient = crFactory.Create<MalleableModule>(client);
            var item = await customResourceClient.ReadNamespacedAsync("default", "test-module");
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(item);
            Assert.Contains("TestClass3", malleableAssembly.Types);
            var type = malleableAssembly.Types["TestClass3"];
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
            type.GetProperty("TestProperty")!.SetValue(instance, "test1");
            var value = (string?)type.GetProperty("TestProperty")!.GetValue(instance);
            Assert.Equal("test1", value);
        }
    }
}