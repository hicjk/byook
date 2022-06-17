using System.Security.Cryptography;
using System.Text;

namespace byook.Controllers;

public class MemberController : Controller
{
    private readonly ByookDbContext context;
    
    public MemberController(ByookDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    [Route("/api/member/sellers")]
    public async Task<IActionResult> IsRegistered([FromQuery] string businessNumber)
    {
        var isFind = await context.Sellers!
            .AsNoTracking()
            .AnyAsync(d => d.SellerId.Equals(businessNumber));

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

        var newModel = model with
        {
            Password = hash
        };

        Console.WriteLine(newModel);

        await context.Consumers!.AddAsync(newModel);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult SellerRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SellerRegister(Seller model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }

        using var sha512 = SHA512.Create();

        var passwordBytes = Encoding.Default.GetBytes(model.Password);
        var passwordHash = sha512.ComputeHash(passwordBytes);
        var hash = BitConverter.ToString(passwordHash).Replace("-", string.Empty);

        var newModel = model with
        {
            Password = hash
        };

        Console.WriteLine(newModel);

        await context.Sellers!.AddAsync(newModel);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    public IActionResult SelectJoin()
    {
        return View();
    }
}