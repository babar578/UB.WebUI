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

        Task<IEnumerable<Mdl_Config_Product>> GetAllAsync();
        Task<Mdl_Config_Product> GetByIdAsync(long id);
        Task<Mdl_Config_Product> AddAsync(Mdl_Config_Product model);
        Task<Mdl_Config_Product> UpdateAsync(Mdl_Config_Product model);
        Task<int> DeleteAsync(long id);
    }
}
