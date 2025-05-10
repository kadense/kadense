using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public interface IMalleableHasModules
    {
        public IEnumerable<string> GetModuleNames();
    }
}