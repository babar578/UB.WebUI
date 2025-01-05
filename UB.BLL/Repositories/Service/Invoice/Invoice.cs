using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UB.BLL.Repositories.Interface.IInvoice;
using UB.DLL.Model;
using Dapper;

namespace UB.BLL.Repositories.Service.Invoice
{
        public class Invoice : IInven_Invoice
        {
            #region "Variables"
            private readonly string _connectionString;
            private IDbConnection Connection => new SqlConnection(_connectionString);
            #endregion

            #region "Constructor"
            public Invoice(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            }
            #endregion

            #region "Methods"
            // Get all invoice headers
            public async Task<IEnumerable<Mdl_Inv_InvoiceHead>> GetAllAsync()
            {
                const string query =
                    @"
            SELECT * 
            FROM Mdl_Inv_InvoiceHead
            ";

                using var connection = Connection;
                var result = await connection.QueryAsync<Mdl_Inv_InvoiceHead>(query);
                return result;
            }

            // Get a single invoice header by its ID
            public async Task<Mdl_Inv_InvoiceHead> GetByIdAsync(int id)
            {
                const string query =
                    @"
            SELECT * 
            FROM Mdl_Inv_InvoiceHead
            WHERE InvoiceId = @InvoiceId
            ";

                using var connection = Connection;
                var result = await connection.QueryFirstOrDefaultAsync<Mdl_Inv_InvoiceHead>(query, new { InvoiceId = id });
                return result ?? throw new KeyNotFoundException("No record found.");
            }

            // Add a new invoice header
            public async Task<Mdl_Inv_InvoiceHead> AddAsync(Mdl_Inv_InvoiceHead model)
            {
                const string query =
                    @"
            INSERT INTO Mdl_Inv_InvoiceHead (CustomerName, InvoiceDate)
            OUTPUT INSERTED.*
            VALUES (@CustomerName, @InvoiceDate)
            ";

                using var connection = Connection;
                var result = await connection.QueryFirstOrDefaultAsync<Mdl_Inv_InvoiceHead>(query, model);
                return result ?? throw new InvalidOperationException("Failed to insert the new record.");
            }

            // Update an existing invoice header
            public async Task<Mdl_Inv_InvoiceHead> UpdateAsync(Mdl_Inv_InvoiceHead model)
            {
                const string query =
                    @"
            UPDATE Mdl_Inv_InvoiceHead
            SET CustomerName = @CustomerName,
                InvoiceDate = @InvoiceDate
            OUTPUT INSERTED.*
            WHERE InvoiceId = @InvoiceId
            ";

                using var connection = Connection;
                var updatedRecord = await connection.QueryFirstOrDefaultAsync<Mdl_Inv_InvoiceHead>(query, model);
                return updatedRecord ?? throw new InvalidOperationException("Failed to update the record.");
            }

            // Delete an invoice header
            public async Task<int> DeleteAsync(int id)
            {
                const string query =
                    @"
            DELETE FROM Mdl_Inv_InvoiceHead
            WHERE InvoiceId = @InvoiceId
            ";

                using var connection = Connection;
                var result = await connection.ExecuteAsync(query, new { InvoiceId = id });
                return (result > 0) ? result : throw new InvalidOperationException("Failed to delete the record.");
            }

            // Get invoice details by InvoiceId
            public async Task<IEnumerable<Mdl_Inv_InvoiceDetails>> GetInvoiceDetailsByInvoiceIdAsync(int invoiceId)
            {
                const string query =
                    @"
            SELECT * 
            FROM Mdl_Inv_InvoiceDetails
            WHERE InvoiceId = @InvoiceId
            ";

                using var connection = Connection;
                var result = await connection.QueryAsync<Mdl_Inv_InvoiceDetails>(query, new { InvoiceId = invoiceId });
                return result;
            }

            // Add a new invoice detail
            public async Task<Mdl_Inv_InvoiceDetails> AddInvoiceDetailAsync(Mdl_Inv_InvoiceDetails model)
            {
                const string query =
                    @"
            INSERT INTO Mdl_Inv_InvoiceDetails (InvoiceId, ProductId, Quantity, UnitPrice)
            OUTPUT INSERTED.*
            VALUES (@InvoiceId, @ProductId, @Quantity, @UnitPrice)
            ";

                using var connection = Connection;
                var result = await connection.QueryFirstOrDefaultAsync<Mdl_Inv_InvoiceDetails>(query, model);
                return result ?? throw new InvalidOperationException("Failed to insert the new record.");
            }

            // Get invoice payments by InvoiceId
            public async Task<IEnumerable<Mdl_Acc_InvoicePayments>> GetInvoicePaymentsByInvoiceIdAsync(int invoiceId)
            {
                const string query =
                    @"
            SELECT * 
            FROM Mdl_Acc_InvoicePayments
            WHERE InvoiceId = @InvoiceId
            ";

                using var connection = Connection;
                var result = await connection.QueryAsync<Mdl_Acc_InvoicePayments>(query, new { InvoiceId = invoiceId });
                return result;
            }

            // Add a new invoice payment
            public async Task<Mdl_Acc_InvoicePayments> AddInvoicePaymentAsync(Mdl_Acc_InvoicePayments model)
            {
                const string query =
                    @"
            INSERT INTO Mdl_Acc_InvoicePayments (InvoiceId, PaymentDate, Amount)
            OUTPUT INSERTED.*
            VALUES (@InvoiceId, @PaymentDate, @Amount)
            ";

                using var connection = Connection;
                var result = await connection.QueryFirstOrDefaultAsync<Mdl_Acc_InvoicePayments>(query, model);
                return result ?? throw new InvalidOperationException("Failed to insert the new payment record.");
            }
            #endregion
        }





    }
