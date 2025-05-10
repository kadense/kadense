using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public interface IMalleableValidated
    {
        public abstract bool IsValid(ILogger logger);
    }
}