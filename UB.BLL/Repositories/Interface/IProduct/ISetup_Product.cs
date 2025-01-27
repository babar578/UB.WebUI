using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UB.DLL.Model;

namespace UB.BLL.Repositories.Interface.IProduct
{
    public interface ISetup_Product
    {

        Task<IEnumerable<Products>> GetAllAsync();
    Task<Products> GetByIdAsync(long Id);
        Task<Products> AddAsync(Products model);
        Task<Products> UpdateAsync(Products model);
        Task<int> DeleteAsync(long id);
    }
}
