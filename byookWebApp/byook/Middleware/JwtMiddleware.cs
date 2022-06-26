using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace byook.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly IConfiguration _configuration;
    private const string _ID_ = "xxxService";
    private const string KEYNAME = "Authorization";

    public JwtMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
    {
        _requestDelegate = requestDelegate;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Cookies.ContainsKey(KEYNAME))
        {
        }

        if(!context.Request.Headers.TryGetValue(KEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Api Key was not provided. (Using ApiKeyMiddleware)");
            return;
        }

        string accessToken = context.Request.Headers[KEYNAME].FirstOrDefault()!.Split(" ").Last();  // authorization bearer 형식의 헤더 키 값으로 넘어옴

        if(!ValidateToken(accessToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("jwt token validation failed");
            return;
        }
        await _requestDelegate(context);
    }

    private bool ValidateToken(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(authSigningKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return (userId == _ID_);
        }
        catch(Exception)
        {
            return false;
        }
    }
}