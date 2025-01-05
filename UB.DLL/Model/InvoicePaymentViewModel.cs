using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UB.DLL.Model
{
    public class InvoicePaymentViewModel
    {


        public int InvoiceId { get; set; }
        public IEnumerable<Mdl_Acc_InvoicePayments> Payments { get; set; }

        // For adding new payments
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
