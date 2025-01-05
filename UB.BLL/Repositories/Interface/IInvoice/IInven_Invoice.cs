using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UB.DLL.Model;

namespace UB.BLL.Repositories.Interface.IInvoice
{
    public interface IInven_Invoice
    {

        // Methods for InvoiceHead
        Task<IEnumerable<Mdl_Inv_InvoiceHead>> GetAllAsync();
        Task<Mdl_Inv_InvoiceHead> GetByIdAsync(int id);
        Task<Mdl_Inv_InvoiceHead> AddAsync(Mdl_Inv_InvoiceHead model);
        Task<Mdl_Inv_InvoiceHead> UpdateAsync(Mdl_Inv_InvoiceHead model);
        Task<int> DeleteAsync(int id);

        // Methods for InvoiceDetails
        Task<IEnumerable<Mdl_Inv_InvoiceDetails>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId);
        Task<Mdl_Inv_InvoiceDetails> AddInvoiceDetailAsync(Mdl_Inv_InvoiceDetails model);

        // Methods for InvoicePayments
        Task<IEnumerable<Mdl_Acc_InvoicePayments>> GetInvoicePaymentsByInvoiceIdAsync(int invoiceId);
        Task<Mdl_Acc_InvoicePayments> AddInvoicePaymentAsync(Mdl_Acc_InvoicePayments model);



    }
}
