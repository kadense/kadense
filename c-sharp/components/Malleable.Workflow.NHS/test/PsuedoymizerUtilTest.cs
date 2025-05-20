using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable.Tests;

namespace Kadense.Malleable.Workflow.NHS.Tests {
    public class PsuedoymizerUtilTest
    {
        [Fact]
        public void TestPseudoOnConverter()
        {
            var mocker = new MalleableMockers();
            var module = mocker.MockModule();
            var converter = mocker.MockConverterModule();
            converter.Spec!.Converters["FromTestInheritedClassToTestClass"].Expressions["TestStringV1"] = "Pseudo.NhsNumber(Source.TestString)";
            var assemblies = new MalleableAssemblyFactory()
                .WithExpressionParameter<PsuedoymizerExpressionUtil>("Pseudo")
                .WithAssembly(module)
                .WithAssembly(converter)
                .GetAssemblies();

            var assembly = assemblies["test-namespace:test-module"];
            var converterAssembly = assemblies["test-namespace:test-converter-module"];
            var instanceType = assembly.Types["TestInheritedClass"];
            var instance = Activator.CreateInstance(instanceType);
            Assert.NotNull(instance);
            instance.GetType().GetProperty("TestString")!.SetValue(instance, "1234567881");
            
            var converterType = converterAssembly.Types["FromTestInheritedClassToTestClass"];
            var util = new PsuedoymizerExpressionUtil();
            var converterInstance = Activator.CreateInstance(converterType, new object[] { new Dictionary<string, object>() { { "Pseudo", util } } });
            var method = converterType.GetMethod("Convert")!;
            var convertedInstance = method.Invoke(converterInstance, new object[] { instance });
            Assert.NotNull(convertedInstance);
            var convertedString = convertedInstance!.GetType().GetProperty("TestStringV1")!.GetValue(convertedInstance);
            Assert.NotNull(convertedString);
            Assert.IsType<string>(convertedString);
            Assert.Equal("b3186f4f0562db43", convertedString);
        }
    }
}