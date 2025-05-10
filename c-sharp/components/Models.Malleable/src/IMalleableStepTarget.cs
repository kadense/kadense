using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public interface IMalleableHasStepTargets
    {
        public IEnumerable<string> GetStepNames();
    }
}