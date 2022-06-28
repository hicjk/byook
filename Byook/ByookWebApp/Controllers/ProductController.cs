namespace ByookWebApp.Controllers;

[Authorize(Policy = nameof(Seller))]
public class ProductController : Controller
{
    
    public IActionResult CreateProduct()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(Product model)
    {
        return View();
    }

    public IActionResult UpdateProduct()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProduct(Product model)
    {
        return View();
    }
}