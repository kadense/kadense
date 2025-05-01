using YamlDotNet.RepresentationModel;
using Xunit.Abstractions;

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
    }
}