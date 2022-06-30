namespace Byook.Utility.Extensions
{
    public static class TokenValidationParametersExtensions
    {
        public static TokenValidationParameters ToTokenValidationParams(this TokenOptions tokenOptions)
        {
            return new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,

                ValidAudience = tokenOptions.Audience,
                ValidIssuer = tokenOptions.Issuer,
                IssuerSigningKey = tokenOptions.SigningKey,

                RequireExpirationTime = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}