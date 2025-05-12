using Xunit.Abstractions;

namespace Kadense.Malleable.Reflection.Tests 
{
    public class MessageEnvelopeTest : KadenseTest
    {
        public MessageEnvelopeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory()]
        [InlineData(null, null, "zz6q+SdIr+vrer+vl6Q4a37G8zD3u6wCalN79Qkz5WQ=")]
        [InlineData("TestEntry", null, "n6VdiCx3+PXF1XnbVapNtobm8Yu6KcQlLytutkcBhdU=")]
        [InlineData("TestEntry", "n6VdiCx3+PXF1XnbVapNtobm8Yu6KcQlLytutkcBhdU=", "bBMoYHlCZGxtka2UMl65aIljdQng+EJDUREdSph7L5Q=")]
        [InlineData("TestEntry2", null, "x5AiAzcHobVdQMyIqRlNk1URtKtx0Mkr60aDd/F0Kw8=")]
        [InlineData("TestEntry2", "n6VdiCx3+PXF1XnbVapNtobm8Yu6KcQlLytutkcBhdU=", "C4aVTjE1NeiZq9mMjZ3X7CFfvaoRIiL/tZ1QvxWrwGM=")]
        public void TestGenerateSignature(string? value, string? previousSignature, string expectedValue)
        {
            var message = new MalleableBasicMockClass
            {
                TestProperty = value
            };

            var envelope = new MalleableEnvelope<MalleableBasicMockClass>(message);

            var result = envelope.GenerateSignatures(previousSignature);
            Assert.True(result);
            Assert.NotNull(envelope.LineageSignature);
            Assert.NotEmpty(envelope.LineageSignature);
            Output.WriteLine($"Generated Signature: {envelope.LineageSignature}");
            Assert.Equal(expectedValue, envelope.LineageSignature);
        }
    }
}