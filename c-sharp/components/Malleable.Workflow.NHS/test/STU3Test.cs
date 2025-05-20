using System.Text.Json;
using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable;
using Kadense.Models.Malleable.Tests;
using Kadense.Malleable.Workflow.NHS.Extensions;

namespace Kadense.Malleable.Workflow.NHS.Tests {
    public class STU3Test
    {
        [Fact]
        public void TestConvert()
        {
            var factory = new MalleableModuleFactory();
            var module = factory.FromFhirSchema("test-namespace", "test-module");
            Assert.NotNull(module);
            Assert.Contains("ReferralRequest", module.Spec!.Classes!.Keys);
            Assert.Contains("identifier", module.Spec.Classes["ReferralRequest"].Properties!.Keys);
            
            var modules = factory.SplitModule(module);
            Assert.NotNull(modules);
            
            var malleableAssemblyFactory = new MalleableAssemblyFactory()
                .WithAssemblies(modules);
            var assemblies = malleableAssemblyFactory.GetAssemblies();
            Assert.NotNull(assemblies);
            var referralRequestType = assemblies.GetType("ReferralRequest");
            var bundleType =  assemblies.GetType("Bundle");
            string json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.NHS.Tests.Resources.Fhir.STU3.FhirReferral.json");
            var referralRequest = JsonSerializer.Deserialize(json, referralRequestType);
            Assert.NotNull(referralRequest);

            json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.NHS.Tests.Resources.Fhir.STU3.FhirReferralGpConnect.json");
            var bundle = JsonSerializer.Deserialize(json, bundleType);
            Assert.NotNull(bundle);
        }
    }
}