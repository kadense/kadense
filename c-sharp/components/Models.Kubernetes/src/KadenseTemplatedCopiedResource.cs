using System.ComponentModel;
using System.Text.RegularExpressions;
using k8s;

namespace Kadense.Models.Kubernetes
{
    public abstract class KadenseTemplatedCopiedResource<T>
    {
        public abstract T ToOriginal(Dictionary<string, string> variables);

        protected string? GetValue(string? templateValue, Dictionary<string, string> variables)
        {
            if(templateValue == null)
                return null;
                
            return Regex.Replace(templateValue, @"\{(\w+)\}", match =>
            {
                var key = match.Groups[1].Value;
                return variables.TryGetValue(key, out var value) ? value : match.Value;
            });
        }

        protected List<string>? GetValue(List<string>? templateValue, Dictionary<string,string> variables)
        {
            if(templateValue == null)
                return null;
            
            return [.. templateValue.Select(i => this.GetValue(i, variables))];
        }

        protected Dictionary<string, string>? GetValue(Dictionary<string, string>? templateValue, Dictionary<string,string> variables)
        {
            if(templateValue == null)
                return null;
            
            var items = new List<KeyValuePair<string, string>>();

            foreach(var i in templateValue)
            {
                var key = this.GetValue(i.Key, variables);
                var value = this.GetValue(i.Value, variables);

                if(key == null)
                    throw new NullReferenceException("Key cannot be null");

                if(value == null)
                    throw new NullReferenceException("Value cannot be null");


                items.Add(KeyValuePair.Create<string, string>(key, value));
            }

            return new Dictionary<string, string>(items);
        }

        protected Dictionary<string, k8s.Models.ResourceQuantity>? GetValueAsResourceQuantity(Dictionary<string, string>? templateValue, Dictionary<string,string> variables)
        {
            if(templateValue == null)
                return null;
            
            var items = new List<KeyValuePair<string, k8s.Models.ResourceQuantity>>();

            foreach(var i in templateValue)
            {
                var key = this.GetValue(i.Key, variables);
                var value = this.GetValue(i.Value, variables);

                if(key == null)
                    throw new NullReferenceException("Key cannot be null");

                if(value == null)
                    throw new NullReferenceException("Value cannot be null");


                items.Add(KeyValuePair.Create<string, k8s.Models.ResourceQuantity>(key, new k8s.Models.ResourceQuantity(value)));
            }

            return new Dictionary<string, k8s.Models.ResourceQuantity>(items);
        }

        protected IList<TDestination>? GetValue<TSource, TDestination>(List<TSource>? templateValue, Dictionary<string, string> variables)
            where TSource : KadenseTemplatedCopiedResource<TDestination>
        {
            if(templateValue == null)
                return null;
            
            return templateValue.Select(i => i.ToOriginal(variables)).ToList();
        }
    }
}