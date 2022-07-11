using Byook.Utility.Interface;
using Byook.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Byook.Controllers
{
    public sealed class MemberController : Controller
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
        public async Task<IActionResult> IsSellerRegistered([FromQuery] string businessNumber)
        {
            IActionResult result = Unauthorized();

            try
            {
                var user = await context.Seller!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id.Equals(businessNumber));

                return Ok(user is not null);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("/api/member/consumers")]
        public async Task<IActionResult> IsConsumerRegistered([FromQuery] string businessNumber)
        {
            try
            {
                var user = await context.Consumer!
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id.Equals(businessNumber));

                return Ok(user is null);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult SellerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerLogin(SellerLoginViewModel user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }

            var hash = GeneratorHash(user.Password);
            var findUser = await context.Seller!.FirstOrDefaultAsync(u => u.Id.Equals(user.SellerId) && u.Password.Equals(hash));

            if(findUser is null)
            {
                return View(nameof(SellerLogin), user);
            }

            var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(user.SellerId, GeneratorClaims(user.SellerId, nameof(Seller)));

            await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal!, accessTokenResult.AuthProperties!);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ConsumerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConsumerLogin(ConsumerLoginViewModel user)
        {
            if(!ModelState.IsValid)
            {
                var errorItem = ModelState.FirstOrDefault(d => d.Value!.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).Key;
                TempData["Error"] = errorItem;

                return View(user);
            }

            var hash = GeneratorHash(user.Password);
            var findUser = await context.Consumer!.FirstOrDefaultAsync(u => u.Id.Equals(user.ConsumerId) && u.Password.Equals(hash));

            if(findUser is null)
            {
                return View(nameof(SellerLogin), user);
            }

            var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(user.ConsumerId, GeneratorClaims(user.ConsumerId, nameof(Consumer)));

            await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal!, accessTokenResult.AuthProperties!);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
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

            await context.Consumer!.AddAsync(model);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ConsumerLogin));
        }

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

            var hash = GeneratorHash(model.Seller!.Password);

            model.Seller!.Password = hash;
            model.Seller.Address = $"{model.Seller.Address} {model.OtherAddress}";

            try
            {
                await context.Seller!.AddAsync(model.Seller!);
                await context.SaveChangesAsync();
            }
            catch(Exception)
            {

            }

            return RedirectToAction(nameof(SellerLogin));
        }

        [Authorize(Policy = nameof(Seller))]
        public IActionResult SellerMyPage()
        {
            return View();
        }

        public IActionResult SelectRegister()
        {
            return View();
        }

        private string GeneratorHash(string password)
        {
            using var sha512 = SHA512.Create();

            var passwordBytes = Encoding.Default.GetBytes(password);
            var passwordHash = sha512.ComputeHash(passwordBytes);
            var hash = BitConverter.ToString(passwordHash).Replace("-", string.Empty);

            return hash;
        }

        private IList<Claim> GeneratorClaims(string id, string role)
        {
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, role),
        };

            return authClaims;
        }
    }
}