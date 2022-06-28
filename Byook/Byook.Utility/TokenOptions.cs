using System.Text;

namespace Byook.Utility;

public sealed class TokenOptions
{
    public SecurityKey? SigningKey { get; }

    public string Issuer { get; } = string.Empty;

    public string Audience { get; } = string.Empty;

    public int TokenExpiryInMinutes { get; }

    public TokenOptions(string issuer, string audience, string signingKey, int expiryInMinute = 30)
    {
        if(string.IsNullOrWhiteSpace(audience))
        {
            throw new ArgumentNullException($"{nameof(Audience)} 변수는 JWT 생성에 필수 요소");
        }

        if(string.IsNullOrWhiteSpace(issuer))
        {
            throw new ArgumentNullException($"{nameof(Issuer)} 변수는 JWT 생성에 필수 요소");
        }

        if(string.IsNullOrWhiteSpace(signingKey))
        {
            throw new ArgumentNullException($"{nameof(Issuer)} 변수는 JWT 생성에 필수 요소");
        }

        Audience = audience;
        Issuer = issuer;
        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        TokenExpiryInMinutes = expiryInMinute;
    }
}

public struct TokenConstants
{
    public const string TokenName = "JWT";
}