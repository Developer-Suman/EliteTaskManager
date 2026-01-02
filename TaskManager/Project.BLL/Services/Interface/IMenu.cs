using Project.BLL.DTOs.Menu;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IMenu
    {
        Task<Result<MenuGetDTOs>> Add(MenuCreatesDTOs menuCreatesDTOs);
        Task<Result<MenuGetDTOs>> Remove(string menuId);
        Task<Result<MenuGetDTOs>> Update(string menuId, MenuUpdateDTOs menuUpdateDTOs);
        Task<Result<MenuGetDTOs>> GetById(string menuId);
        Task<Result<List<MenuGetDTOs>>> GetAll();
        Task<Result<List<MenuGetDTOs>>> GetMenusBySubModulesId(string subModuleId);
    }
}
