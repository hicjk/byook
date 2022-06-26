using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace byook.Utility
{
    public sealed class TokenWithClaimsPrincipal
    {
        public string AccessToken { get; set; } = string.Empty;

        public ClaimsPrincipal? ClaimsPrincipal { get; set; }

        public AuthenticationProperties? AuthProperties { get; set; }
    }
}
