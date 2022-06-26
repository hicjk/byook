using byook.Models;
using byook.Utility.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace byook.Utility;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly TokenOptions tokenOptions;

    public JwtTokenGenerator(TokenOptions tokenOptions)
    {
        this.tokenOptions = tokenOptions ?? throw new ArgumentNullException($"An instance of valid {nameof(TokenOptions)} must be passed in order to generate a JWT!"); ;
    }

    public string GenerateAccessToken<TUser>(TUser user, IEnumerable<Claim> claims) where TUser : User
    {
        var expiration = TimeSpan.FromMinutes(tokenOptions.TokenExpiryInMinutes);
        var jwt = new JwtSecurityToken(issuer: tokenOptions.Issuer,
                                       audience: tokenOptions.Audience,
                                       claims: MergeUserClaimsWithDefaultClaims(user, claims),
                                       notBefore: DateTime.UtcNow,
                                       expires: DateTime.UtcNow.Add(expiration),
                                       signingCredentials: new SigningCredentials(
                                           tokenOptions.SigningKey,
                                           SecurityAlgorithms.HmacSha256));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return accessToken;
    }

    public TokenWithClaimsPrincipal GenerateAccessTokenWithClaimsPrincipal<TUser>(TUser user, IEnumerable<Claim> userClaims) where TUser : User
    {
        var userClaimList = userClaims.ToList();
        var accessToken = GenerateAccessToken(user, userClaimList);

        return new TokenWithClaimsPrincipal()
        {
            AccessToken = accessToken,
            ClaimsPrincipal = ClaimsPrincipalFactory.CreatePrincipal(MergeUserClaimsWithDefaultClaims(user, userClaimList),authenticationType: CookieAuthenticationDefaults.AuthenticationScheme, roleType: nameof(Seller)),
            AuthProperties = CreateAuthProperties(accessToken)
        };
    }

    private static AuthenticationProperties CreateAuthProperties(string accessToken)
    {
        var authProps = new AuthenticationProperties();
        authProps.StoreTokens(new[]
        {
            new AuthenticationToken()
            {
                Name = TokenConstants.TokenName,
                Value = accessToken
            }
        });

        return authProps;
    }

    private static IEnumerable<Claim> MergeUserClaimsWithDefaultClaims<TUser>(TUser user, IEnumerable<Claim> userClaims) where TUser : User
    {
        var claims = new List<Claim>(userClaims)
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

        return claims;
    }
}