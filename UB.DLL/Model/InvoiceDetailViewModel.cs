using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace UB.DLL.Model
{
    public class InvoiceDetailViewModel
    {
        public Mdl_Inv_InvoiceHead Invoice { get; set; }

        // For InvoiceDetails
        public IEnumerable<Mdl_Inv_InvoiceDetails> InvoiceDetails { get; set; }
        //public IEnumerable<Mdl_Config_Product> Products { get; set; }  // Add this property
        public IEnumerable<SelectListItem> Products { get; set; } // New property for SelectListItems

        public int InvoiceDetailId {  get; set; }   
        public int InvoiceId { get; set; }  // Add this line
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
