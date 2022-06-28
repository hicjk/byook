namespace Byook.Utility;

public sealed class ClaimsPrincipalFactory
{
    public static ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims, string? authenticationType = "Password", string? roleType = "Recipient")
    {
        var claimsPrincipal = new ClaimsPrincipal();

        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, authenticationType, ClaimTypes.Name, roleType));

        return claimsPrincipal;
    }
}