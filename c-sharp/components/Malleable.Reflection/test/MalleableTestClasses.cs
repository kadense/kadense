using Kadense.Models.Malleable;
using Newtonsoft.Json;

namespace Kadense.Malleable.Reflection.Tests;

[MalleableClass("test-namespace", "test-internal-module", "MalleableTestClass")]
public class MalleableTestClass : MalleableBase
{
    [JsonProperty("testString")]
    public string? TestString { get; set; }
}


[MalleableClass("test-namespace", "test-internal-module", "MalleableTestClassConverted")]
public class MalleableTestClassConverted : MalleableBase
{
    [JsonProperty("testStringV2")]
    public string? TestStringV2 { get; set; }
}

[MalleableConverter("test-namespace", "test-internal-module", "FromMalleableTestClassToMalleableTestClassConverted")]
[MalleableConvertFrom("test-namespace", "test-internal-module", "MalleableTestClass")]
[MalleableConvertTo("test-namespace", "test-internal-module", "MalleableTestClassConverted")]
public class FromMalleableTestClassToMalleableTestClassConverted : MalleableConverterBase<MalleableTestClass, MalleableTestClassConverted>
{
    public FromMalleableTestClassToMalleableTestClassConverted(IDictionary<string, object> parameters) : base(parameters)
    {

    }

    public override MalleableTestClassConverted Convert(MalleableTestClass input)
    {
        return new MalleableTestClassConverted
        {
            TestStringV2 = input.TestString
        };
    }
}