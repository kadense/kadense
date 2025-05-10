using Kadense.Malleable.Reflection;
using Kadense.Malleable.Reflection.Tests;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Models.Malleable.Tests;

namespace Kadense.Malleable.Workflow.Tests {
    public class IfElseProcessorTests
    {
        [Fact]
        public void TestProcess()
        {
            var mocker = new MalleableMockers();
            var moduleDefinition = mocker.MockModule();
            var converterDefinition = mocker.MockConverterModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.CreateAssembly(moduleDefinition);
            var malleableAssemblyList = new Dictionary<string, MalleableAssembly>(){
                { malleableAssembly.Name, malleableAssembly }
            };
            var converterAssembly = malleableAssemblyFactory.CreateAssembly(converterDefinition, malleableAssemblyList);
            malleableAssemblyList.Add(converterAssembly.Name, converterAssembly);
            var workflow = mocker.MockWorkflow();
            var type = typeof(IfElseProcessor<>).MakeGenericType(new Type[] { malleableAssembly.Types["TestInheritedClass"] });
            MalleableWorkflowContext context = new MalleableWorkflowContext(workflow, malleableAssemblyList, true);
            var processor = (MalleableWorkflowProcessor)Activator.CreateInstance(type, new object[] { context, "TestConditional" })!;
            var instance = (MalleableBase)Activator.CreateInstance(malleableAssembly.Types["TestInheritedClass"])!;
            instance.GetType().GetProperty("TestString")!.SetValue(instance, "test1");
            (var destination, var processed) = processor.Process(instance);
            Assert.IsType(malleableAssembly.Types["TestInheritedClass"], processed);
            Assert.NotNull(destination);
            Assert.Equal("TestStep", destination);

            instance.GetType().GetProperty("TestString")!.SetValue(instance, "test2");
            (destination, processed) = processor.Process(instance);
            Assert.IsType(malleableAssembly.Types["TestInheritedClass"], processed);
            Assert.Null(destination);
        }
    }
}