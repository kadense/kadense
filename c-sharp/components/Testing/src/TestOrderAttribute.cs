namespace Kadense.Testing
{
    // Attribute to specify the execution order of test methods.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestOrderAttribute : Attribute
    {
        // The order in which the test method should be executed.
        public int Order { get; }
        
        // Constructor to initialize the order value.
        public TestOrderAttribute(int order)
        {
            Order = order;
        }
    }
}