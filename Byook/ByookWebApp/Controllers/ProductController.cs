using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace ByookWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ByookDbContext context;

        public ProductController(ByookDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = context.Product!;

            return View(products);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile? file)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                if(file is not null)
                {
                    var rootPath = hostEnvironment.WebRootPath;
                    var extension = Path.GetExtension(file.FileName);
                    var uploadPath = Path.Combine(rootPath, @"saveImages\Products\");

                    var saveImageName = Path.Combine(model.CreateDate.ToString("yyyyMMddHHmmss"), extension).Replace("\\", string.Empty);

                    using var fs = new FileStream(Path.Combine(uploadPath, saveImageName), FileMode.Create);

                    await file.CopyToAsync(fs);

                    model.ImageUrl = $@"/saveImages/Products/{saveImageName}";
                }

                model.SellerId = User.Claims.FirstOrDefault(d => d.Type.Equals(ClaimTypes.NameIdentifier))!.Value;

                await context.Product!.AddAsync(model);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}