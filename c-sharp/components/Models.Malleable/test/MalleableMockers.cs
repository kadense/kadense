using k8s.Models;

namespace Kadense.Models.Malleable.Tests
{
    public class MalleableMockers
    {
        public MalleableConverterModule MockConverterModule()
        {
            return new MalleableConverterModule
            {
                Metadata = new V1ObjectMeta
                {
                    Name = "test-converter-module",
                    NamespaceProperty = "test-namespace",
                    Labels = new Dictionary<string, string>
                    {
                        { "app", "test-app" }
                    }
                },
                Spec = new MalleableConverterModuleSpec
                {
                    Converters = new Dictionary<string, MalleableTypeConverter>
                    {
                        {
                            "FromTestInheritedClassToTestClass",
                            new MalleableTypeConverter
                            {
                                From = new MalleableTypeReference()
                                {
                                    ClassName = "TestInheritedClass",
                                    ModuleName = "test-module",
                                    ModuleNamespace = "test-namespace"
                                },
                                To = new MalleableTypeReference()
                                {
                                    ClassName = "ConvertedClass",
                                    ModuleName = "test-module",
                                    ModuleNamespace = "test-namespace"
                                },
                                Expressions = new Dictionary<string, string>
                                {
                                    { "TestStringV1", "Source.TestString" },
                                    { "TestStringPrefix", "Source.TestString.Substring(0, 1)" },
                                },
                            }
                        }
                    }
                }
            };
        }

        public MalleableModule MockModule()
        {
            return new MalleableModule
            {
                Metadata = new V1ObjectMeta
                {
                    Name = "test-module",
                    NamespaceProperty = "test-namespace",
                    Labels = new Dictionary<string, string>
                    {
                        { "app", "test-app" }
                    }
                },
                Spec = new MalleableModuleSpec
                {
                    Description = "Test module description",
                    Classes = new Dictionary<string, MalleableClass>
                    {
                        {
                            "TestClass",
                            new MalleableClass {
                                Description = "Test class description",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "TestList",
                                        new MalleableProperty
                                        {
                                            PropertyType = "list",
                                            SubType = "string"
                                        }
                                    }
                                },
                            }
                        },
                        {
                            "TestInheritedClass",
                            new MalleableClass
                            {
                                BaseClass = "TestClass",
                                IdentifierExpression = "Input.TestString",
                                Description = "Test of Inherited class",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "TestString",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            PropertyType = "string",
                                        }
                                    }
                                },
                            }
                        },
                        {
                            "TestReferenceClass",
                            new MalleableClass
                            {
                                BaseClass = "TestClass",
                                Description = "Test of Inherited class 2",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "TestReference",
                                        new MalleableProperty
                                        {
                                            Description = "Test Reference",
                                            PropertyType = "TestInheritedClass",
                                        }
                                    },
                                    {
                                        "DictionaryReference",
                                        new MalleableProperty
                                        {
                                            Description = "Test Reference",
                                            PropertyType = "dictionary",
                                            SubType = "TestInheritedClass",
                                        }
                                    },
                                },
                            }
                        },
                        {
                            "ConvertedClass",
                            new MalleableClass
                            {
                                Description = "Test of Converted class",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "TestStringV1",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            PropertyType = "string",
                                        }
                                    },
                                    {
                                        "TestStringPrefix",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            PropertyType = "string",
                                        }
                                    },
                                },
                            }
                        }
                   }
                }
            };
        }
    }
}