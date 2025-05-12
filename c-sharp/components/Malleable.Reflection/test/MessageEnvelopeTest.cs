using Xunit.Abstractions;

namespace Kadense.Malleable.Reflection.Tests 
{
    public class MessageEnvelopeTest : KadenseTest
    {
        public MessageEnvelopeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory()]
        [InlineData(null, null, "Pm6DwwEyIJX1yONGQm2xnS9rSD7wiRUMgkv1o25HQAM=")]
        [InlineData("TestEntry", null, "5jsDoWVcH+qBviFVlvbvoJJpWtQQ+FSPr+9b8wSNkfs=")]
        [InlineData("TestEntry", "5jsDoWVcH+qBviFVlvbvoJJpWtQQ+FSPr+9b8wSNkfs=", "BIZYSkmUDL92NTsrwSJ+V2XFTEVB09qGWqvsVjDFwnM=")]
        [InlineData("TestEntry2", null, "7IB8LO8CFkpLT/cKRCO4eUG7STxd8TbT3Mw6E5ledEg=")]
        [InlineData("TestEntry2", "5jsDoWVcH+qBviFVlvbvoJJpWtQQ+FSPr+9b8wSNkfs=", "sD/NtZLyb2RNOXY6FGeQt8ecLBUu37FJtE7u6uKahRE=")]
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