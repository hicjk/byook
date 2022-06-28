using Byook.Models;
using Byook.Utility.Interface;

namespace Byook.Utility;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly TokenOptions tokenOptions;

    public JwtTokenGenerator(TokenOptions tokenOptions)
    {
        this.tokenOptions = tokenOptions ?? throw new ArgumentNullException($"An instance of valid {nameof(TokenOptions)} must be passed in order to generate a JWT!"); ;
    }

    public string GenerateAccessToken(string user, IEnumerable<Claim> claims)
    {
        var expiration = TimeSpan.FromMinutes(tokenOptions.TokenExpiryInMinutes);
        var jwt = new JwtSecurityToken(issuer: tokenOptions.Issuer,
                                       audience: tokenOptions.Audience,
                                       claims: MergeUserClaimsWithDefaultClaims(user, claims),
                                       notBefore: DateTime.Now,
                                       expires: DateTime.Now.Add(expiration),
                                       signingCredentials: new SigningCredentials(
                                           tokenOptions.SigningKey,
                                           SecurityAlgorithms.HmacSha256));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return accessToken;
    }

    public TokenWithClaimsPrincipal GenerateAccessTokenWithClaimsPrincipal(string user, IEnumerable<Claim> userClaims)
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

    private static IEnumerable<Claim> MergeUserClaimsWithDefaultClaims(string user, IEnumerable<Claim> userClaims)
    {
        var claims = new List<Claim>(userClaims)
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

        return claims;
    }
}