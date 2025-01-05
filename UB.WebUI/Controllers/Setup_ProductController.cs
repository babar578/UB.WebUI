using Microsoft.AspNetCore.Mvc;
using UB.BLL.Repositories.Interface.IProduct;
using UB.DLL.Model;
using System.Threading.Tasks;

namespace UB.WebUI.Controllers
{
    public class Setup_ProductController : Controller
    {
        private readonly ISetup_Product _productRepository;

        public Setup_ProductController(ISetup_Product productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products); // Corresponds to a Razor View: Index.cshtml
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View(); // Corresponds to Create.cshtml
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mdl_Config_Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: /Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Corresponds to Edit.cshtml
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Mdl_Config_Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: /Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Corresponds to Delete.cshtml
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Corresponds to Details.cshtml
        }
    }
}
