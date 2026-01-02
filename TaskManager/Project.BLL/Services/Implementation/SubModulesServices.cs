
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Menu;
using Project.BLL.DTOs.SubModules;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class SubModulesServices : ISubModules
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;


        public SubModulesServices(
            ApplicationDbContext applicationDbContext,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager)
        {
            _context = applicationDbContext;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;

        }
        public async Task<Result<SubModulesGetDTOs>> Add(SubModulesCreateDTOs subModulesCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    if (subModulesCreateDTOs.role is not null && !(await _roleManager.RoleExistsAsync(subModulesCreateDTOs.role)))
                    {
                        return Result<SubModulesGetDTOs>.Failure("NotFound", "Role doesnot Exists");
                    }

                    var submodulesData = new SubModules(
                    newId,
                    subModulesCreateDTOs.name,
                    subModulesCreateDTOs.iconUrl,
                    subModulesCreateDTOs.targetUrl,
                    subModulesCreateDTOs.role,
                    subModulesCreateDTOs.moduleId,
                    subModulesCreateDTOs.rank,
                    subModulesCreateDTOs.isActive
                    );


                    await _unitOfWork.Repository<SubModules>().AddAsync(submodulesData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var resultDTOs = new SubModulesGetDTOs(
                        submodulesData.Id,
                        submodulesData.Name,
                        submodulesData.iconUrl,
                        submodulesData.TargetUrl,
                        submodulesData.Role,
                        submodulesData.Rank,
                        submodulesData.IsActive
                        );


                    return Result<SubModulesGetDTOs>.Success(resultDTOs);


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while adding sub Modules");
                }
            }
        }

        public Task<Result<List<SubModulesGetDTOs>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Result<SubModulesGetDTOs>> GetById(string subModuleId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SubModulesGetDTOs>> Remove(string subModuleId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SubModulesGetDTOs>> Update(string subModuleId, SubModulesUpdateDTOs subModulesUpdateDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
