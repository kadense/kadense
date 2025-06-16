using System.Collections.Generic;
using System.Security.Claims;

namespace Kadense.Web.Security
{
    public interface IClaimsTransformer
    {
        IEnumerable<Claim> TransformClaims(OAuthUserInfo userInfo, IEnumerable<Claim> originalClaims);
    }
}
