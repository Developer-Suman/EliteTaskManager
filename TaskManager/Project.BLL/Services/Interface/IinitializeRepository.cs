using Project.BLL.DTOs;
using Project.BLL.DTOs.Initialize;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IinitializeRepository
    {
        Task<Result<InitializeDTOs>> InitializeAsync(); 
        Task<Result<string>> ControllerActionSeeder();
    }
}
