using System.Diagnostics;
using System.IO.Compression;

namespace Kadense.Models.Malleable
{
    public class MalleableModuleSpec
    {
        /// <summary>
        /// The module description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Classes for this module
        /// </summary>
        [JsonPropertyName("classes")]
        public Dictionary<string, MalleableClass>? Classes { get; set; }

        public IEnumerable<string> GetReferencedModules()
        {
            if (Classes == null)
                return new List<string>();

            var list = new List<string>();

            Classes
                .ToList()
                .ForEach(x =>
                {
                    (var isReference, var classRef) = x.Value.TryGetBaseClassReference();
                    if (isReference)
                        list.Add(classRef!.GetQualifiedModuleName());


                    x.Value.GetReferencedTypes()
                        .ToList()
                        .ForEach(y =>
                        {
                            list.Add(y.GetQualifiedModuleName());
                        });
                });
            return list.Distinct();
        }

        public bool ClassReferences(string className, string otherClassName)
        {
            var isDescendant = ClassIsDescendant(className, otherClassName);
            if (isDescendant)
                return true;


            return Classes![className].Properties!.Any(p => p.Value.Type == otherClassName);
        }

        public bool ClassIsDescendant(string className, string baseClassName)
        {
            if (Classes == null)
                throw new Exception($"No classes found in module");

            var classItem = Classes[className];

            if (classItem.BaseClass == null)
                return false;

            if (classItem.BaseClass == baseClassName)
                return true;

            return ClassIsDescendant(classItem.BaseClass, baseClassName);
        }

        public IList<string> GetAncestors(string className, IList<string>? ancestors = null)
        {

            if (ancestors == null)
                ancestors = new List<string>();

            ancestors.Add(className);

            if (Classes == null)
                throw new Exception($"No classes found in module");

            var classItem = Classes[className];

            if (classItem.BaseClass == null)
                return ancestors;

            return GetAncestors(classItem.BaseClass, ancestors);
        }

        public IList<MalleableModuleSpec> SplitClasses(int threshold = 50)
        {
            if (Classes == null)
                throw new Exception($"No classes found in module");

            var modules = new List<MalleableModuleSpec>();

            var newItems = Classes
            .Where(c => c.Value.BaseClass == null)
            .Where(c => c.Value.Properties!.All(p => !p.Value.IsComplexType()));

            var remainingItems = Classes
                .Where(c => !newItems.Any(o => o.Key == c.Key)); // not already added

            modules.Add(new MalleableModuleSpec()
            {
                Classes = newItems.ToDictionary(x => x.Key, x => x.Value)
            });


            while (remainingItems.Count() > 0)
            {
                var firstItem = remainingItems.First();
                var relatedClasses = this.GetRelatedClasses(firstItem.Key, newItems.ToList());
                newItems = newItems.Concat(relatedClasses);
                remainingItems = remainingItems.Where(c => !relatedClasses.Any(o => o.Key == c.Key));
                var currentModule = modules.Last();
                if (currentModule.Classes!.Count() > threshold)
                {
                    modules.Add(new MalleableModuleSpec()
                    {
                        Classes = relatedClasses.ToDictionary(x => x.Key, x => x.Value)
                    });
                }
                else
                {
                    currentModule.Classes = currentModule.Classes!
                        .Concat(relatedClasses)
                        .ToDictionary(x => x.Key, x => x.Value);
                }
            }

            /*.ThenBy(c => c, new MalleableComparer<KeyValuePair<string, MalleableClass>>((x, y) =>
                {
                    var xRefsY = this.ClassReferences(x.Key, y.Key);
                    var yRefsX = this.ClassReferences(y.Key, x.Key);

                    if (xRefsY && !yRefsX)
                        return yFirst;

                    if (!xRefsY && yRefsX)
                        return xFirst;

                    return 0;
                }))
                .ThenBy(c =>
                {
                    var ancestors = GetAncestors(c.Key).Reverse();
                    var path = $"{string.Join("/", ancestors)}/0000";
                    return path;
                })
                .ToList();*/

            return modules;
        }

        protected IList<KeyValuePair<string, MalleableClass>> GetRelatedClasses(string className, IList<KeyValuePair<string, MalleableClass>> alreadyImported, List<KeyValuePair<string, MalleableClass>>? newItems = null)
        {
            var @class = Classes![className];

            if (newItems == null)
                newItems = new List<KeyValuePair<string, MalleableClass>>();

            newItems.Add(new KeyValuePair<string, MalleableClass>(className, @class));
            alreadyImported.Add(new KeyValuePair<string, MalleableClass>(className, @class));

            if (@class.BaseClass != null)
            {
                if (!alreadyImported.Any(x => x.Key == @class.BaseClass))
                {
                    var baseClass = Classes!.First(x => x.Key == @class.BaseClass);
                    GetRelatedClasses(baseClass.Key, alreadyImported, newItems);
                }
            }

            if (@class.DiscriminatorClass != null)
            {
                if (!alreadyImported.Any(x => x.Key == @class.DiscriminatorClass))
                {
                    var discriminatorClass = Classes!.First(x => x.Key == @class.DiscriminatorClass);
                    GetRelatedClasses(discriminatorClass.Key, alreadyImported, newItems);
                }
            }

            this.Classes!
                .Where(x => x.Value.BaseClass == className)
                .ToList()
                .ForEach(x =>
                {
                    if (!alreadyImported.Any(y => y.Key == x.Key))
                    {
                        GetRelatedClasses(x.Key, alreadyImported, newItems);
                    }
                });
            
            if (@class.Properties != null)
            {
                foreach (var prop in @class.Properties)
                {
                    if (prop.Value.IsComplexType())
                    {
                        var (isComplexType, complexType) = prop.Value.TryGetComplexTypeReference();
                        if (isComplexType && !alreadyImported.Any(x => x.Key == complexType!.ClassName))
                        {
                            var propClass = Classes!.First(x => x.Key == complexType!.ClassName);
                            GetRelatedClasses(propClass.Key, alreadyImported, newItems);
                        }
                    }
                }
            }
            return newItems;
        }

        public IList<KeyValuePair<string, MalleableClass>> GetClassesForBuild()
        {
            if (Classes == null)
                return new List<KeyValuePair<string, MalleableClass>>();

            var added = Classes.Where(x => x.Value.BaseClass == null);
            var items = added.ToList();

            while (added.Count() > 0)
            {
                added = Classes
                    .Where(x => !items.Any(y => y.Key == x.Key) // not already added
                    && x.Value.BaseClass != null  // has a base class
                    && items.Any(y => y.Key == x.Value.BaseClass)) // base class is already added
                    .ToList();
                items.AddRange(added);
            }

            var referencedTypes = Classes
                .Where(x => !items.Any(y => y.Key == x.Key)) // not already added
                .Where(x =>
                {
                    (var isReference, var classRef) = x.Value.TryGetBaseClassReference();
                    return isReference;
                });
            items.AddRange(referencedTypes);

            var remaining = Classes
                .Where(x => !items.Any(y => y.Key == x.Key)); // not already added

            if (remaining.Count() > 0)
            {
                var firstRemaining = remaining.First();
                throw new Exception($"Class {firstRemaining.Key} refers to a base class {firstRemaining.Value.BaseClass} that is not defined in this module. Please check the module definition.");
            }

            return items;
        }
    }
}