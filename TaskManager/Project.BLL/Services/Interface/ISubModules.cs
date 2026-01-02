using Project.BLL.DTOs.SubModules;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface ISubModules
    {
        Task<Result<SubModulesGetDTOs>> Add(SubModulesCreateDTOs subModulesCreateDTOs);
        Task<Result<SubModulesGetDTOs>> Remove(string subModuleId);
        Task<Result<SubModulesGetDTOs>> Update(string subModuleId, SubModulesUpdateDTOs subModulesUpdateDTOs);
        Task<Result<SubModulesGetDTOs>> GetById(string subModuleId);
        Task<Result<List<SubModulesGetDTOs>>> GetAll();
       
    }
}
