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
using System.Net.NetworkInformation;

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
        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return await connection.QueryAsync<Products>("sp_GetAllProducts", commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while retrieving all products", ex);
            }
        }
        public async Task<Products> GetByIdAsync(long id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var product = await connection.QueryFirstOrDefaultAsync<Products>(
                    "GetProductById",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                return product;
            }
            catch (SqlException ex)
            {
                
                throw new Exception("An error occurred while retrieving the product", ex);
            }
        }


        public async Task<Products> AddAsync(Products product)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync("sp_InsertProduct", new { product.Name, product.Price, product.Quantity }, commandType: CommandType.StoredProcedure);
                if (result > 0)
                {
                    return product; 
                }
                else
                {
                    throw new Exception("Failed to add product.");
                }
            }
            catch (SqlException ex)
            {
               
                throw new Exception("An error occurred while adding the product", ex);
            }
        }

        public async Task<Products> UpdateAsync(Products product)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync("sp_UpdateProduct", product, commandType: CommandType.StoredProcedure);
                if (result > 0)
                {
                    return product; 
                }
                else
                {
                    throw new Exception("Failed to update product.");
                }
            }
            catch (SqlException ex)
            {
                
                throw new Exception("An error occurred while updating the product", ex);
            }
        }

        public async Task<int> DeleteAsync(long id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync("sp_DeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);
                return result; 
            }
            catch (SqlException ex)
            {
                
                throw new Exception("An error occurred while deleting the product", ex);
            }
        }



    }
}
