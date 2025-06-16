namespace Kadense.Models.Malleable.Tests;

public class WorkflowFactoryTests
{
    [Fact]
    public void TestWorkflowFactory()
    {
        MalleableWorkflowFactory factory  = new MalleableWorkflowFactory("test-namespace", "test-workflow");

        var module = factory
        .AddApi("TestApi")
            .SetUnderlyingType("test-namespace", "test-module", "TestInheritedClass")
            .AddStep("TestConditional", "IfElse")
                .AddIfCondition("Input.TestString == \"test1\"", "TestStep", "Convert")
                    .SetConverter("test-namespace", "test-converter-module", "FromTestInheritedClassToTestClass")
                    .AddNextStep("WriteApi", "WriteApi")
                        .SetParameter("baseUrl", "http://localhost:8080")
                        .SetParameter("Path", "\"test\"")
                    .EndStep() // End WriteApi step
                .EndStep() // End TestStep
            .EndStep() // End TestConditional step
        .EndApi();
        

    }
}