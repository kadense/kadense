using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;

namespace Kadense.Malleable.Workflow.Email.Tests {
    public class SendEmailProcessorTests
    {
        [Fact]
        public void SendEmailTest()
        {
            var mockers = new MailMockers();
            var module = mockers.MockModule();
            var converter = mockers.MockConverterModule();
            var workflow = mockers.MockWorkflow();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.WithNewAssembly(module);
            malleableAssemblyFactory.WithAssembly(converter);
            var processorType = typeof(SendEmailProcessor<,>).MakeGenericType(new Type[] { malleableAssembly.Types["ConvertedClass"], malleableAssembly.Types["ConvertedClass"] });
            var context = new MalleableWorkflowContext(workflow, malleableAssemblyFactory.GetAssemblies(), true);
            context.Providers.Add("DefaultSendEmailProvider", new DummySendEmailProvider(
                new DummySendEmailProviderOptions((sender, recipient, subject, bodyHtml, bodyPlainText) =>
                {
                    Assert.Equal("enquiries@kadense.io", sender);
                    Assert.Equal("enquiries@kadense.io", recipient);
                    Assert.Equal("Test Email from Malleable Workflow for test123", subject);
                    Assert.Equal("<h1>Test Email</h1><p>This is a test email from Malleable Workflow for test123.</p>", bodyHtml);
                    Assert.Equal("This is a test email from Malleable Workflow for test123", bodyPlainText);
                })
            ));
            var processor = (MalleableWorkflowProcessor)Activator.CreateInstance(processorType, new object[] { context, "SendEmailStep" })!;
            var instance = (MalleableBase)Activator.CreateInstance(malleableAssembly.Types["ConvertedClass"])!;
            instance.GetType().GetProperty("TestStringV1")!.SetValue(instance, "test123");
            (var destination, var converted) = processor.Process(instance);
            Assert.NotNull(converted);
            Assert.Null(destination);

            
        }
    }
}