
using Project.BLL.DTOs.Modules;
using Project.BLL.DTOs.ModulesFlags;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IModule
    {
        Task<Result<ModulesGetDTOs>> Add(ModulesCreateDTOs modulesCreateDTOs);
        Task<Result<ModulesGetDTOs>> GetModulesWithDetails(string moduleId);
        Task<Result<GetModulesRoles>> AssignModulesToRoles(string roleId, IEnumerable<string> modulesId);
        Task<Result<GetModulesRoles>> RemoveModulesFromRoles(string roleId, IEnumerable<string> modulesId);
        Task<Result<List<ModulesGetDTOsWithUsers>>> GetNavigationMenuByUser(string userId);
        Task<Result<List<ModulesGetDTOs>>> GetModulesByUserId(string userId);
        Task<Result<List<bool>>> UpdateModulesFlagByModulesId(string userId, List<UpdateModulesFlag> updateModulesFlag);
        Task<Result<ModulesGetDTOs>> GetById(string moduleId);
        Task<Result<List<ModulesGetDTOs>>> GetAll();

        Task<Result<List<ModulesGetDTOs>>> GetModulesByRolesId(string roleId);
    }
}
