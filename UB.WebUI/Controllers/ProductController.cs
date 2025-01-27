using Microsoft.AspNetCore.Mvc;
using System.Text;
using UB.BLL.Repositories.Interface.IProduct;
using UB.DLL.Model;

namespace UB.WebUI.Controllers
{
    public class ProductController : Controller
    {

        private readonly ISetup_Product _productRepository;

        public ProductController(ISetup_Product productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products); 
        }

        public IActionResult Create()
        {
            return View(); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DLL.Model.Products product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DLL.Model.Products product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
           var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); 
        }

        
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DownloadCsv()
        {
            
            var products = await _productRepository.GetAllAsync();

         
            var csvContent = GenerateCsv(products);

         
            var fileBytes = Encoding.UTF8.GetBytes(csvContent);

         
            return File(fileBytes, "text/csv", "products.csv");
        }

        private string GenerateCsv(IEnumerable<Products> products)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,Price,Quantity"); 

            foreach (var product in products)
            {
                sb.AppendLine($"{product.Id},{product.Name},{product.Price},{product.Quantity}");
            }

            return sb.ToString();
        }

    }
}
