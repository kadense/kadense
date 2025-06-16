using Kadense.Malleable.Reflection;
using Kadense.Malleable.Reflection.Tests;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Tests;

public class InternalMocker
{
    public virtual MalleableAssemblyFactory GetAssemblyFactory()
    {
        return new MalleableAssemblyFactory()
            .WithType<MalleableTestClass>()
            .WithType<MalleableTestClassConverted>()
            .WithConverterType<FromMalleableTestClassToMalleableTestClassConverted>();
    }
    public virtual MalleableWorkflow GetWorkflow()
    {
        MalleableWorkflowFactory factory = new MalleableWorkflowFactory("test-namespace", "test-workflow");

        return factory
        .AddApi("TestApi")
            .SetUnderlyingType("test-namespace", "test-internal-module", "MalleableTestClass")
            .AddStep("TestConditional", "IfElse")
                .AddIfCondition("Input.TestString == \"test1\"", "TestStep", "Convert")
                    .SetConverter("test-namespace", "test-internal-module", "FromMalleableTestClassToMalleableTestClassConverted")
                    .AddNextStep("WriteApi", "WriteApi")
                        .SetParameter("baseUrl", "http://localhost:8080")
                        .SetParameter("Path", "\"test\"")
                    .EndStep() // End WriteApi step
                .EndStep() // End TestStep
            .EndStep() // End TestConditional step
        .EndApi()
        .EndWorkflow();
    }
}