using Byook.ViewModels;
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

        [Authorize(Policy = nameof(Seller))]
        public async Task<IActionResult> Index()
        {
            var sellerId = User.Claims.FirstOrDefault(d => d.Type.Equals(ClaimTypes.NameIdentifier))!.Value;

            var products = context.Product!.Where(d => d.SellerId.Equals(sellerId));

            return View(await products.ToListAsync());
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Policy = nameof(Seller))]
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
                model.SellerId = GetSellerId();

                await UploadFileAndReturnUrl(model, file);

                await context.Product!.AddAsync(model);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task UploadFileAndReturnUrl(Product model, IFormFile? file)
        {
            if(file is not null)
            {
                var rootPath = hostEnvironment.WebRootPath;
                var extension = Path.GetExtension(file.FileName);
                var uploadPath = Path.Combine(rootPath, @"saveImages\Products\");

                var saveImageName = Path.Combine(model.SellerId, model.CreateDate.ToString("yyyyMMddHHmmss"), extension).Replace("\\", string.Empty);

                using var fs = new FileStream(Path.Combine(uploadPath, saveImageName), FileMode.Create);

                await file.CopyToAsync(fs);

                model.ImageUrl = $@"/saveImages/Products/{saveImageName}";
            }
        }

        private void DeleteImage(string imageUrl)
        {
            if(System.IO.File.Exists($"{hostEnvironment.WebRootPath}{imageUrl}"))
            {
                System.IO.File.Delete($"{hostEnvironment.WebRootPath}{imageUrl}");
            }
        }

        // GET: ProductController/Edit/5
        [Authorize(Policy = nameof(Seller))]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Product!.FirstOrDefaultAsync(d => d.Id == id);

            if(product is null)
            {
                return RedirectToAction(nameof(Index));
            }
            
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Seller))]
        public async Task<IActionResult> Edit(Product model, IFormFile? file)
        {
            try
            {
                model.SellerId = GetSellerId();

                DeleteImage(model.ImageUrl);

                await UploadFileAndReturnUrl(model, file);

                context.Product!.Update(model);
                await context.SaveChangesAsync();

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
        public async Task<IActionResult> Delete([FromBody] ProductDeleteViewModel model)
        {
            try
            {
                var product = await context.Product!.FirstOrDefaultAsync(d => d.SellerId == model.SellerId && d.Id == model.Productid);

                if(product is null || !product.ImageUrl.Equals(model.ImageUrl))
                {
                    return BadRequest("존재하지 않는 상품이거나 저장 되어있는 이미지의 경로가 다릅니다.");
                }

                DeleteFile(product.ImageUrl);

                context.Product!.Remove(product);

                await context.SaveChangesAsync();

                return Json("성공");
            }
            catch
            {
                return BadRequest("상품 삭제 중 에러 발생");
            }
        }

        private void DeleteFile(string filePath)
        {
            var fullPath = $"{hostEnvironment.WebRootPath}{filePath}";

            if(System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        } 

        private string GetSellerId()
        {
            return User.Claims.FirstOrDefault(d => d.Type.Equals(ClaimTypes.NameIdentifier))!.Value;
        }
    }
}