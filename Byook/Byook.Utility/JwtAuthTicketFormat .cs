using Microsoft.AspNetCore.DataProtection;

namespace Byook.Utility;

public sealed class JwtAuthTicketFormat : ISecureDataFormat<AuthenticationTicket>
{
    private const string Algorithm = SecurityAlgorithms.HmacSha256;
    private readonly TokenValidationParameters validationParameters;
    private readonly IDataSerializer<AuthenticationTicket> ticketSerializer;
    private readonly IDataProtector dataProtector;

    public JwtAuthTicketFormat(TokenValidationParameters validationParameters, IDataSerializer<AuthenticationTicket> ticketSerializer, IDataProtector dataProtector)
    {
        this.validationParameters = validationParameters ?? throw new ArgumentNullException($"{nameof(validationParameters)} cannot be null");
        this.ticketSerializer = ticketSerializer ?? throw new ArgumentNullException($"{nameof(ticketSerializer)} cannot be null"); ;
        this.dataProtector = dataProtector ?? throw new ArgumentNullException($"{nameof(dataProtector)} cannot be null");
    }

    public AuthenticationTicket? Unprotect(string? protectedText)
    {
        return Unprotect(protectedText, null);
    }

    public AuthenticationTicket? Unprotect(string? protectedText, string? purpose)
    {
        AuthenticationTicket? authTicket;

        try
        {
            authTicket = ticketSerializer.Deserialize(dataProtector.Unprotect(Base64UrlTextEncoder.Decode(protectedText!)));

            var embeddedJwt = authTicket?.Properties?
                .GetTokenValue(TokenConstants.TokenName);

            new JwtSecurityTokenHandler().ValidateToken(embeddedJwt, validationParameters, out var token);

            if(token is not JwtSecurityToken jwt)
            {
                throw new SecurityTokenValidationException("JWT token was found to be invalid");
            }

            if(!jwt.Header.Alg.Equals(Algorithm, StringComparison.Ordinal))
            {
                throw new ArgumentException($"Algorithm must be '{Algorithm}'");
            }
        }
        catch(Exception)
        {
            return null;
        }

        return authTicket;
    }

    public string Protect(AuthenticationTicket data)
    {
        return Protect(data, null);
    }

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        var array = ticketSerializer.Serialize(data);

        return Base64UrlTextEncoder.Encode(dataProtector.Protect(array));
    }
}