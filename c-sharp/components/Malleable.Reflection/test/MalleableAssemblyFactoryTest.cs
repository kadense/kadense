using YamlDotNet.RepresentationModel;
using Xunit.Abstractions;
using Kadense.Client.Kubernetes;
using Kadense.Models.Malleable;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Kadense.Malleable.Reflection.Tests 
{
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
            Assert.Contains("FromTestInheritedClassToTestClass", converterAssembly.Types);

            var converterType = converterAssembly.Types["FromTestInheritedClassToTestClass"];
            var convertedClassType = malleableAssembly.Types["ConvertedClass"];

            var converterInstance = Activator.CreateInstance(converterType);
            var method = converterType.GetMethod("Convert")!;
            
            var convertedInstance = method.Invoke(converterInstance, new object[] { instance });
            Assert.NotNull(convertedInstance);
            var testString = convertedClassType.GetProperty("TestStringV1")!.GetValue(convertedInstance);
            var testStringPrefix = convertedClassType.GetProperty("TestStringPrefix")!.GetValue(convertedInstance);
            Assert.Equal("test1", testString);
            Assert.Equal("t", testStringPrefix);

            Assert.IsAssignableFrom<IMalleableIdentifiable>(instance);
            var identifiable = (IMalleableIdentifiable)instance;
            var identifier = identifiable.GetIdentifier();
            Assert.NotNull(identifier);
            Assert.Equal("test1", identifier);
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

        [Fact]
        public void TestGetIdentifier()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var type = malleableAssembly.Types["TestInheritedClass"];
            var instance = Activator.CreateInstance(type);
            type.GetProperty("TestString")!.SetValue(instance, "test1");
            type.GetProperty("TestList")!.SetValue(instance, new List<string> { "test2", "test3" });

            Assert.NotNull(instance);

            // Log the emitted IL code for debugging
            var identifiable = (IMalleableIdentifiable)instance;
            try
            {
                var identifier = identifiable.GetIdentifier();
                Assert.NotNull(identifier);
                Assert.Equal("test1", identifier);
            }
            catch (InvalidProgramException ex)
            {
                Console.WriteLine("InvalidProgramException occurred: " + ex.Message);
                throw;
            }
        }

        
        [Fact]
        public void TestPolymorphism()
        {
            var mocker = new MalleableMockers();
            var malleableModule = mocker.MockModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(malleableModule);
            var instanceType = malleableAssembly.Types["ContainerOfPolymorphicClasses"];
            var instance = Activator.CreateInstance(instanceType);
            var type1 = malleableAssembly.Types["PolymorphicDerivedClass1"];
            var type2 = malleableAssembly.Types["PolymorphicDerivedClass2"];
            var polyBaseClass = malleableAssembly.Types["PolymorphicBaseClass"];

            Assert.NotNull(instance);

            instanceType.GetProperty("TestStringV2")!.SetValue(instance, "test123");
            var listType = typeof(List<>).MakeGenericType(polyBaseClass);
            var listInstance = Activator.CreateInstance(listType)!;
            instanceType.GetProperty("PolymorphicList")!.SetValue(instance, listInstance);
            var addMethod = listType.GetMethod("Add")!;

            var polymorphicInstance1 = Activator.CreateInstance(type1)!;
            var polymorphicInstance2 = Activator.CreateInstance(type2)!;
            type1.GetProperty("DerivedStringProperty")!.SetValue(polymorphicInstance1, "testString");
            type2.GetProperty("DerivedIntProperty")!.SetValue(polymorphicInstance2, 123);
            addMethod.Invoke(listInstance, new object[] { polymorphicInstance1 });
            addMethod.Invoke(listInstance, new object[] { polymorphicInstance2 });

            var expectedValue = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Reflection.Tests.Resources.Polymorphic.json");
            var typeInfoResolver = new MalleablePolymorphicTypeResolver();
            typeInfoResolver.MalleableAssembly.Add(malleableAssembly);
            var json = JsonSerializer.Serialize(instance, new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = typeInfoResolver,
            });
            Assert.Equal(expectedValue, json);
        }
    }
}