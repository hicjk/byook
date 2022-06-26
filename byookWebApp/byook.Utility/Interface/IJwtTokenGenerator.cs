using byook.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace byook.Utility.Interface;

public interface IJwtTokenGenerator
{
    TokenWithClaimsPrincipal GenerateAccessTokenWithClaimsPrincipal<TUser>(TUser user, IEnumerable<Claim> userClaims) where TUser : User;
    string GenerateAccessToken<TUser>(TUser user, IEnumerable<Claim> userClaims) where TUser : User;
}