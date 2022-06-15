using System.Security.Cryptography;
using System.Text;

namespace byook.Controllers;

public class MemberController : Controller
{
    private ByookDbContext context;
    
    public MemberController(ByookDbContext context)
    {
        this.context = context;
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
    public IActionResult ConsumerJoin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConsumerJoin(Consumer model)
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
    public IActionResult SellerJoin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SellerJoin(Seller model)
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