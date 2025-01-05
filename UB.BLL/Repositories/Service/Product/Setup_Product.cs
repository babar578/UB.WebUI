using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UB.BLL.Repositories.Interface.IProduct;
using UB.DLL.Model;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace UB.BLL.Repositories.Service.Product
{
    public class Setup_Product : ISetup_Product
    {
        #region "Variables"
        private readonly string _connectionString;
        private IDbConnection Connection => new SqlConnection(_connectionString);
        #endregion

        #region "Constructor"
        public Setup_Product(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
        }
        #endregion

        #region "Methods"
        public async Task<IEnumerable<Mdl_Config_Product>> GetAllAsync()
        {
            const string query =
                @"
                SELECT *
                FROM Mdl_Config_Product
                ";

            using var connection = Connection;
            var result = await connection.QueryAsync<Mdl_Config_Product>(query);
            return result;
        }

        public async Task<Mdl_Config_Product> GetByIdAsync(long id)
        {
            const string query =
                @"
                SELECT *
                FROM Mdl_Config_Product
                WHERE ProductId = @ProductId
                ";

            using var connection = Connection;
            var result = await connection.QueryFirstOrDefaultAsync<Mdl_Config_Product>(query, new { ProductId = id });
            return result ?? throw new KeyNotFoundException("No record found.");
        }

        public async Task<Mdl_Config_Product> AddAsync(Mdl_Config_Product model)
        {
            const string query =
                 @"
        INSERT INTO Mdl_Config_Product (
            Name, Description, Price
        )
        OUTPUT INSERTED.*
        VALUES (
            @Name, @Description, @Price
        )
        ";

            using var connection = Connection;
            var result = await connection.QueryFirstOrDefaultAsync<Mdl_Config_Product>(query, model);
            return result ?? throw new InvalidOperationException("Failed to insert the new record.");
        }

        public async Task<Mdl_Config_Product> UpdateAsync(Mdl_Config_Product model)
        {
            const string query =
                @"
                UPDATE Mdl_Config_Product
                SET Name = @Name,
                    Description = @Description,
                    Price = @Price
                OUTPUT INSERTED.*
                WHERE ProductId = @ProductId
                ";

            using var connection = Connection;
            var updatedRecord = await connection.QueryFirstOrDefaultAsync<Mdl_Config_Product>(query, model);
            return updatedRecord ?? throw new InvalidOperationException("Failed to update the record.");
        }

        public async Task<int> DeleteAsync(long id)
        {
            const string query =
                @"
                DELETE FROM Mdl_Config_Product
                WHERE ProductId = @ProductId
                ";

            using var connection = Connection;
            var result = await connection.ExecuteAsync(query, new { ProductId = id });
            return (result > 0) ? result : throw new InvalidOperationException("Failed to delete the record.");
        }
        #endregion

    }
}
