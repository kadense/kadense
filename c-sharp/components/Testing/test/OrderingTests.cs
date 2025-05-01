using Xunit.Abstractions;

namespace Kadense.Testing.Tests {
    public class OrderingTests : KadenseTest
    { 
        public OrderingTests(ITestOutputHelper output) : base(output)
        {
        }
        private static bool _TestA = false;
        private static bool _TestZ = false;
        private static bool _TestY = false;

        [TestOrder(1)]
        [Fact]
        public void TestZ()
        {
            _TestZ = true;

            Assert.False(_TestY);
            Assert.False(_TestA);
        }

        

        [TestOrder(3)]
        [Fact]
        public void TestA()
        {
            _TestA = true;

            Assert.True(_TestZ);
            Assert.True(_TestY);
        }


        [TestOrder(2)]
        [Fact]
        public void TestY()
        {
            _TestY = true;

            Assert.True(_TestY);
            Assert.False(_TestA);
        }
    }
}