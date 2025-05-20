using System.Text.Json;
using k8s.Models;

namespace Kadense.Models.Malleable.Tests
{
    public class MalleableMockers
    {
        public MalleableWorkflow MockWorkflow()
        {
            return new MalleableWorkflow
            {
                Metadata = new V1ObjectMeta
                {
                    Name = "test-workflow",
                    NamespaceProperty = "test-namespace",
                    Labels = new Dictionary<string, string>
                    {
                        { "app", "test-app" }
                    }
                },
                Spec = new MalleableWorkflowSpec
                {
                    Description = "Test workflow description",
                    APIs = new Dictionary<string, MalleableWorkflowApi>
                    {
                        {
                            "TestApi",
                            new MalleableWorkflowApi
                            {
                                ApiType = "Ingress",
                                UnderlyingType = new MalleableTypeReference(){
                                    ClassName = "TestInheritedClass",
                                    ModuleName = "test-module",
                                    ModuleNamespace = "test-namespace"
                                },
                                IngressOptions = new MalleableWorkflowApiOptions()
                                {
                                    NextStep = "TestConditional",
                                }
                            }
                        }
                    },
                    Steps = new Dictionary<string, MalleableWorkflowStep>
                    {
                        {
                            "TestConditional",
                            new MalleableWorkflowStep
                            {
                                Action = "IfElse",
                                IfElseOptions = new MalleableWorkflowIfElseActionOptions
                                {
                                    Expressions = new List<MalleableWorkflowIfElseExpression>
                                    {
                                        new MalleableWorkflowIfElseExpression
                                        {
                                            Expression = "Input.TestString == \"test1\"",
                                            NextStep = "TestStep"
                                        }
                                    },
                                }
                            }
                        },
                        {
                            "TestStep",
                            new MalleableWorkflowStep
                            {
                                Action = "Convert",
                                ConverterOptions = new MalleableWorkflowConverterActionOptions
                                {
                                    Converter = new MalleableConverterReference
                                    {
                                        ConverterName = "FromTestInheritedClassToTestClass",
                                        ModuleName = "test-converter-module",
                                        ModuleNamespace = "test-namespace"
                                    },
                                    NextStep = "WriteApi",
                                },
                            }
                        },
                        {
                            "WriteApi",
                            new MalleableWorkflowStep
                            {
                                Action = "WriteApi",
                                Options = new MalleableWorkflowStandardActionOptions
                                {
                                    Parameters = new Dictionary<string, string>
                                    {
                                        { "baseUrl", "http://localhost:8080" },
                                        { "Path", "\"test\"" }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

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

        public MalleableModule MockFhirModule()
        {
            var json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Models.Malleable.Tests.Resources.FhirResourceExample.json");
            var module = JsonSerializer.Deserialize<MalleableModule>(json)!;
            return module;
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
                                            Type = "list",
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
                                            Type = "string",
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
                                            Type = "TestInheritedClass",
                                        }
                                    },
                                    {
                                        "DictionaryReference",
                                        new MalleableProperty
                                        {
                                            Description = "Test Reference",
                                            Type = "dictionary",
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
                                            Type = "string",
                                        }
                                    },
                                    {
                                        "TestStringPrefix",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            Type = "string",
                                        }
                                    },
                                },
                            }
                        },
                        {
                            "PolymorphicBaseClass",
                            new MalleableClass
                            {
                                Description = "Test of Polymorphic base class",
                                DiscriminatorProperty = "id",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "BaseStringProperty",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            Type = "string",
                                        }
                                    },
                                },
                            }
                        },
                        {
                            "PolymorphicDerivedClass1",
                            new MalleableClass
                            {
                                BaseClass = "PolymorphicBaseClass",
                                Description = "Test of Polymorphic derived class",
                                TypeDiscriminator = "DerivedString",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "DerivedStringProperty",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            Type = "string",
                                        }
                                    },
                                },
                            }
                        },
                        {
                            "PolymorphicDerivedClass2",
                            new MalleableClass
                            {
                                BaseClass = "PolymorphicBaseClass",
                                Description = "Test of Polymorphic derived class",
                                TypeDiscriminator = "DerivedInt",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "DerivedIntProperty",
                                        new MalleableProperty
                                        {
                                            Description = "integer property",
                                            Type = "int",
                                        }
                                    },
                                },
                            }
                        },
                        {
                            "ContainerOfPolymorphicClasses",
                            new MalleableClass
                            {
                                Description = "Test of Converted class",
                                Properties = new Dictionary<string, MalleableProperty>
                                {
                                    {
                                        "TestStringV2",
                                        new MalleableProperty
                                        {
                                            Description = "string property",
                                            Type = "string",
                                        }
                                    },
                                    {
                                        "PolymorphicList",
                                        new MalleableProperty
                                        {
                                            Description = "list of PolymorphicBaseClass",
                                            Type = "list",
                                            SubType = "PolymorphicBaseClass"
                                        }
                                    },
                                },
                            }
                        }
                   }
                }
            };
        }

        public List<MalleableModule> MockModules()
        {            
            return new List<MalleableModule>()
            { 
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-tc",
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
                                                Type = "list",
                                                SubType = "string"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-tic",
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
                                "TestInheritedClass",
                                new MalleableClass
                                {
                                    BaseClass = "test-namespace:composite-tc:TestClass",
                                    IdentifierExpression = "Input.TestString",
                                    Description = "Test of Inherited class",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "TestString",
                                            new MalleableProperty
                                            {
                                                Description = "string property",
                                                Type = "string",
                                            }
                                        }
                                    },
                                }
                            }
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-trc",
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
                                "TestReferenceClass",
                                new MalleableClass
                                {
                                    BaseClass = "test-namespace:composite-tc:TestClass",
                                    Description = "Test of Inherited class 2",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "TestReference",
                                            new MalleableProperty
                                            {
                                                Description = "Test Reference",
                                                Type = "test-namespace:composite-tic:TestInheritedClass",
                                            }
                                        },
                                        {
                                            "DictionaryReference",
                                            new MalleableProperty
                                            {
                                                Description = "Test Reference",
                                                Type = "dictionary",
                                                SubType = "test-namespace:composite-tic:TestInheritedClass",
                                            }
                                        },
                                    },
                                }
                            }
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-cc",
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
                                                Type = "string",
                                            }
                                        },
                                        {
                                            "TestStringPrefix",
                                            new MalleableProperty
                                            {
                                                Description = "string property",
                                                Type = "string",
                                            }
                                        },
                                    },
                                }
                            }
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-pbc",
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
                                "PolymorphicBaseClass",
                                new MalleableClass
                                {
                                    Description = "Test of Polymorphic base class",
                                    DiscriminatorProperty = "id",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "BaseStringProperty",
                                            new MalleableProperty
                                            {
                                                Description = "string property",
                                                Type = "string",
                                            }
                                        },
                                    },
                                }
                            }
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-pdc1",
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
                                "PolymorphicDerivedClass1",
                                new MalleableClass
                                {
                                    BaseClass = "test-namespace:composite-pbc:PolymorphicBaseClass",
                                    Description = "Test of Polymorphic derived class",
                                    TypeDiscriminator = "DerivedString",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "DerivedStringProperty",
                                            new MalleableProperty
                                            {
                                                Description = "string property",
                                                Type = "string",
                                            }
                                        },
                                    },
                                }
                            },
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-pdc2",
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
                                "PolymorphicDerivedClass2",
                                new MalleableClass
                                {
                                    BaseClass = "test-namespace:composite-pbc:PolymorphicBaseClass",
                                    Description = "Test of Polymorphic derived class",
                                    TypeDiscriminator = "DerivedInt",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "DerivedIntProperty",
                                            new MalleableProperty
                                            {
                                                Description = "integer property",
                                                Type = "int",
                                            }
                                        },
                                    },
                                }
                            },
                        }
                    }
                },
                new MalleableModule
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = "composite-copc",
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
                                "ContainerOfPolymorphicClasses",
                                new MalleableClass
                                {
                                    Description = "Test of Converted class",
                                    Properties = new Dictionary<string, MalleableProperty>
                                    {
                                        {
                                            "TestStringV2",
                                            new MalleableProperty
                                            {
                                                Description = "string property",
                                                Type = "string",
                                            }
                                        },
                                        {
                                            "PolymorphicList",
                                            new MalleableProperty
                                            {
                                                Description = "list of PolymorphicBaseClass",
                                                Type = "list",
                                                SubType = "test-namespace:composite-pbc:PolymorphicBaseClass"
                                            }
                                        },
                                    },
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}