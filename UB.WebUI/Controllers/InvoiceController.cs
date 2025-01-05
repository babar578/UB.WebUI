using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UB.BLL.Repositories.Interface.IInvoice;
using UB.DLL.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using UB.BLL.Repositories.Interface.IProduct;


namespace UB.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInven_Invoice _invoiceService;
        private readonly ISetup_Product _productService;
        
        // Constructor 2
        public InvoiceController(ISetup_Product productService, IInven_Invoice invoiceService)
        {
            _productService = productService;
            _invoiceService = invoiceService;
        }

       

        // GET: Invoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return View(invoices);
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the main invoice details (InvoiceHead)
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Fetch the related InvoiceDetails
            var invoiceDetails = await _invoiceService.GetInvoiceDetailsByInvoiceIdAsync(id);

            // Fetch the products for the dropdown
            var products = await _productService.GetAllAsync();
            if (products == null)
            {
                products = new List<Mdl_Config_Product>();  // Initialize to empty list if null
            }

            var productList = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = p.Name
            }).ToList();

            // Create a ViewModel to combine the data
            var viewModel = new InvoiceDetailViewModel
            {
                Invoice = invoice,
                InvoiceDetails = invoiceDetails,
                Products = productList  // Pass the products to the view model
            };

            return View(viewModel);
        }


    

        // GET: Invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerName,InvoiceDate")] Mdl_Inv_InvoiceHead invoice)
        {
            if (ModelState.IsValid)
            {
                var createdInvoice = await _invoiceService.AddAsync(invoice);
                return RedirectToAction(nameof(Details), new { id = createdInvoice.InvoiceId });
            }
            return View(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> EditDetail(int detailId)
        {
            var invoiceDetails = await _invoiceService.GetInvoiceDetailsByInvoiceIdAsync(detailId);
            

            // Find the specific invoice detail using the detail ID
            var invoiceDetail = invoiceDetails.FirstOrDefault(d => d.InvoiceDetailId == detailId);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            var model = new InvoiceDetailViewModel
            {
                InvoiceDetailId = invoiceDetail.InvoiceDetailId,
                InvoiceId = invoiceDetail.InvoiceId,
                ProductId = invoiceDetail.ProductId,
                Quantity = invoiceDetail.Quantity,
                UnitPrice = invoiceDetail.UnitPrice,
                Products = (await _productService.GetAllAsync()).Select(p => new SelectListItem
                {
                    Value = p.ProductId.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(model);
        }


     


        // GET: Invoice/Edit/5
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var invoice = await _invoiceService.GetByIdAsync(id);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(invoice);
        //}

        //// POST: Invoice/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,CustomerName,InvoiceDate")] Mdl_Inv_InvoiceHead invoice)
        //{
        //    if (id != invoice.InvoiceId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _invoiceService.UpdateAsync(invoice);
        //        }
        //        catch (Exception)
        //        {
        //            if (await _invoiceService.GetByIdAsync(id) == null)
        //            {
        //                return NotFound();
        //            }
        //            throw;
        //        }
        //        return RedirectToAction(nameof(Details), new { id });
        //    }
        //    return View(invoice);
        //}

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

 
        // Add InvoiceDetail
        // GET: Invoice/AddDetail/{invoiceId}

        //[HttpPost]
        public async Task<IActionResult> AddDetail(int invoiceId)
        {
            // Fetch the products from the database
            var products = await _productService.GetAllAsync();

            // Map the products to SelectListItems
            var productList = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = p.Name
            }).ToList();

            var viewModel = new InvoiceDetailViewModel
            {
                InvoiceId = invoiceId,
                Products = productList // Assign the SelectListItem list to the Products property
            };

            return View(viewModel);
        }



        // POST: Invoice/AddDetail
 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddDetail(InvoiceDetailViewModel model)
{
          
   
        // Add the invoice detail
        var invoiceDetail = new Mdl_Inv_InvoiceDetails
        {
            InvoiceId = model.InvoiceId,
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            UnitPrice = model.UnitPrice
        };

        await _invoiceService.AddInvoiceDetailAsync(invoiceDetail);

        return RedirectToAction(nameof(Details), new { id = model.InvoiceId });
    
    return View(model); // Return to the same view with validation errors if invalid
}

        public async Task<IActionResult> Payments(int invoiceId)
        {
            // Fetch invoice to ensure it exists
            var invoice = await _invoiceService.GetByIdAsync(invoiceId);
            if (invoice == null) return NotFound();

            // Fetch payments for the invoice
            var payments = await _invoiceService.GetInvoicePaymentsByInvoiceIdAsync(invoiceId);

            // Create the ViewModel
            var viewModel = new InvoicePaymentViewModel
            {
                InvoiceId = invoiceId,
                Payments = payments
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(InvoicePaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payment = new Mdl_Acc_InvoicePayments
                {
                    InvoiceId = model.InvoiceId,
                    PaymentDate = model.PaymentDate,
                    Amount = model.Amount
                };

                await _invoiceService.AddInvoicePaymentAsync(payment);

                return RedirectToAction("Payments", new { invoiceId = model.InvoiceId });
            }

            return View(model); // Return the same view with validation errors if invalid
        }
    }
}
