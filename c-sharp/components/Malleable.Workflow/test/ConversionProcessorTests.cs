using Kadense.Malleable.Reflection;
using Kadense.Malleable.Reflection.Tests;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Models.Malleable;
using Kadense.Models.Malleable.Tests;

namespace Kadense.Malleable.Workflow.Tests {
    public class ConversionProcessorTests
    {
        [Fact]
        public void TestConvert()
        {
            var mocker = new MalleableMockers();
            var moduleDefinition = mocker.MockModule();
            var converterDefinition = mocker.MockConverterModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.WithNewAssembly(moduleDefinition);
            var malleableAssemblyList = new Dictionary<string, MalleableAssembly>(){
                { malleableAssembly.Name, malleableAssembly }
            };
            var converterAssembly = malleableAssemblyFactory.WithNewAssembly(converterDefinition);
            malleableAssemblyList.Add(converterAssembly.Name, converterAssembly);
            var workflow = mocker.MockWorkflow();
            var type = typeof(ConversionProcessor<,>).MakeGenericType(new Type[] { malleableAssembly.Types["TestInheritedClass"], malleableAssembly.Types["ConvertedClass"] });
            MalleableWorkflowContext context = new MalleableWorkflowContext(workflow, malleableAssemblyList, true);
            var processor = (MalleableWorkflowProcessor)Activator.CreateInstance(type, new object[] { context, "TestStep" })!;
            var instance = (MalleableBase)Activator.CreateInstance(malleableAssembly.Types["TestInheritedClass"])!;
            instance.GetType().GetProperty("TestString")!.SetValue(instance, "test1");
            (var destination, var converted) = processor.Process(instance);
            Assert.IsType(malleableAssembly.Types["ConvertedClass"], converted);
        }
    }
}