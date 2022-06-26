using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace byook.Utility
{
    public sealed class ClaimsPrincipalFactory
    {
        public static ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims, string? authenticationType = "Password", string? roleType = "Recipient")
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, authenticationType, ClaimTypes.Name, roleType));

            return claimsPrincipal;
        }
    }
}
