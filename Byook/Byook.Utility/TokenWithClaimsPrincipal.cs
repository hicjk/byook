namespace Byook.Utility;

public sealed class TokenWithClaimsPrincipal
{
    public string AccessToken { get; set; } = string.Empty;

    public ClaimsPrincipal? ClaimsPrincipal { get; set; }

    public AuthenticationProperties? AuthProperties { get; set; }
}