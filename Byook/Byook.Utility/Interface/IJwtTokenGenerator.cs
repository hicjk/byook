using System.Security.Claims;

namespace Byook.Utility.Interface;

public interface IJwtTokenGenerator
{
    TokenWithClaimsPrincipal GenerateAccessTokenWithClaimsPrincipal(string user, IEnumerable<Claim> userClaims);
    string GenerateAccessToken(string user, IEnumerable<Claim> userClaims);
}