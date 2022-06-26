using byook.DataAccess;
using byook.Models;
using byook.Utility;
using byook.Utility.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace byook.Controllers;

public class MemberController : Controller
{
    private readonly ByookDbContext context;
    private readonly IConfiguration configuration;
    private readonly IJwtTokenGenerator tokenGenerator;

    public MemberController(ByookDbContext context, IConfiguration configuration, IJwtTokenGenerator tokenGenerator)
    {
        this.context = context;
        this.configuration = configuration;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpGet]
    [Route("/api/member/sellers")]
    public async Task<IActionResult> IsRegistered([FromQuery] string businessNumber)
    {
        var isFind = await context.Sellers!
            .AsNoTracking()
            .AnyAsync(d => d.Id.Equals(businessNumber));

        return Ok(isFind);
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult SellerLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SellerLogin(User user)
    {
        var hash = GeneratorHash(user);
        var findUser = await context.Sellers!.FirstOrDefaultAsync(u => u.Id.Equals(user.Id) && u.Password.Equals(hash));

        if(findUser is null)
        {
            return View(user);
        }

        var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(user, GeneratorClaims(user));

        //await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal!, accessTokenResult.AuthProperties);
        await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal!, accessTokenResult.AuthProperties!);

        //var claims = GeneratorClaims(user);

        //var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsidentity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SellerLogout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ConsumerRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConsumerRegister(Consumer model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }

        using var sha512 = SHA512.Create();

        var passwordBytes = Encoding.Default.GetBytes(model.Password);
        var passwordHash = sha512.ComputeHash(passwordBytes);
        var hash = BitConverter.ToString(passwordHash).Replace("-", string.Empty);

        await context.Consumers!.AddAsync(model);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult SellerRegister()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SellerRegister(SellerRegisterViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }

        var hash = GeneratorHash(model.Seller!);

        model.Seller!.Password = hash;

        await context.Sellers!.AddAsync(model.Seller!);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    public IActionResult SelectJoin()
    {
        return View();
    }
    
    private string GeneratorHash<TUser>(TUser user) where TUser : User
    {
        using var sha512 = SHA512.Create();

        var passwordBytes = Encoding.Default.GetBytes(user.Password);
        var passwordHash = sha512.ComputeHash(passwordBytes);
        var hash = BitConverter.ToString(passwordHash).Replace("-", string.Empty);

        return hash;
    }

    private IList<Claim> GeneratorClaims(User user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, nameof(Seller)),
        };

        return authClaims;
    }

    private string GeneratorToken<TUser>(TUser user, IConfiguration configuration) where TUser : User
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddSeconds(5),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Subject = new ClaimsIdentity(GeneratorClaims(user), "local"),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        });
        //// JWT 토큰 생성
        //var token = new JwtSecurityToken(
        //    issuer: configuration["Jwt:Issuer"],
        //    audience: configuration["Jwt:Audience"],
        //    expires: DateTime.Now.AddSeconds(10),
        //    claims: GeneratorClaims(user),
        //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //);

        return handler.WriteToken(token);
    }
}