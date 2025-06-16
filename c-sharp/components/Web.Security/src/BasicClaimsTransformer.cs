using System.Collections.Generic;
using System.Security.Claims;

namespace Kadense.Web.Security
{
    public class BasicClaimsTransformer : IClaimsTransformer
    {
        public IEnumerable<Claim> TransformClaims(OAuthUserInfo userInfo, IEnumerable<Claim> originalClaims)
        {
            return new List<Claim>(originalClaims);
        }
    }
}
