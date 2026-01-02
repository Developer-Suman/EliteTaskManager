using Project.BLL.DTOs.Menu;
using Project.BLL.DTOs.Modules;
using Project.BLL.DTOs.SubModules;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Reflection;
using Project.BLL.DTOs.ModulesFlags;
using static System.Formats.Asn1.AsnWriter;

namespace Project.BLL.Services.Implementation
{
    public class ModuleServices : IModule
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ModuleServices(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
        {
            _context = applicationDbContext;
            _unitOfWork = unitOfWork;

        }
        public async Task<Result<ModulesGetDTOs>> Add(ModulesCreateDTOs modulesCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var modulesData = new Modules(
                    newId,
                    modulesCreateDTOs.Name,
                    modulesCreateDTOs.Role,
                    modulesCreateDTOs.TargetUrl,
                    modulesCreateDTOs.isActive
                    );

                    await _unitOfWork.Repository<Modules>().AddAsync(modulesData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new ModulesGetDTOs(
                        modulesData.Id,
                        modulesData.Name,
                        modulesData.Role,
                        modulesData.TargetUrl,
                        modulesData.IsActive);

                    //modulesData.SubModules.Select(sm => new SubModulesGetDTOs(
                    //    sm.Id,
                    //    sm.Name,
                    //    sm.iconUrl,
                    //    sm.TargetUrl,
                    //    sm.Role,
                    //    sm.Rank,
                    //    sm.IsActive
                    //    )).ToList());
                    //.ToList(),
                    //     modulesData.SubModules.SelectMany(sm =>sm.Menu.Select(menu=> new MenuGetDTOs(
                    //        menu.Id,
                    //        menu.Name,
                    //        menu.IconUrl,
                    //        menu.TargetUrl,
                    //        menu.Role,
                    //        menu.Rank,
                    //        menu.IsActive
                    //        )
                    //    )
                    //).ToList());

                    return Result<ModulesGetDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An Exception occured while Adding Modules");

                }
            }
        }

        public async Task<Result<GetModulesRoles>> AssignModulesToRoles(string roleId, IEnumerable<string> modulesId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var exisistingAssignment = await _context.RoleModules
                        .Where(rm => rm.Id == roleId)
                        .ToListAsync();

                    if (exisistingAssignment.Any())
                    {
                        //Determinie existing modulr IDs
                        var existingModulesIds = exisistingAssignment.Select(rm => rm.ModuleId).ToList();

                        //Identify modules to remove
                        var modulesToRemove = exisistingAssignment
                            .Where(rm => !modulesId.Contains(rm.ModuleId))
                            .ToList();

                        if (modulesToRemove.Any())
                        {
                            _context.RoleModules.RemoveRange(modulesToRemove);
                        }
                    }

                    //Determine new Modules To Add
                    var newModuleIds = modulesId.Except(exisistingAssignment.Select(rm => rm.ModuleId));
                    foreach (var moduleId in newModuleIds)
                    {
                        var roleModuleExists = await _unitOfWork.Repository<RoleModule>()
                    .AnyAsync(rm => rm.RoleId == roleId && rm.ModuleId == moduleId);


                        if (!roleModuleExists)
                        {
                            var roleModule = new RoleModule()
                            {
                                Id = Guid.NewGuid().ToString(), // Generate a unique identifier
                                RoleId = roleId,
                                ModuleId = moduleId
                            };

                            await _unitOfWork.Repository<RoleModule>().AddAsync(roleModule);
                        }
                    }

                    await _context.SaveChangesAsync();

                    scope.Complete();


                    var resultDTOs = new GetModulesRoles(
                    roleId,
                    newModuleIds.Select(id => id.ToString()).ToList()
                    );

                    return Result<GetModulesRoles>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while Assigining Roles");
                }
            }
        }

        public Task<Result<List<ModulesGetDTOs>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Result<ModulesGetDTOs>> GetById(string moduleId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<ModulesGetDTOs>>> GetModulesByRolesId(string roleId)
        {
            try
            {
                //Fetch RolesModules and associated Modules in a single query
                var modulesWithRoles = await _unitOfWork.Repository<RoleModule>()
                    .GetConditonalAsync(
                    x => x.RoleId == roleId,
                    query => query.Include(rm => rm.Modules));

                var resultDTOs = modulesWithRoles
                    .Where(rm => rm.Modules is not null)
                    .Select(rm => new ModulesGetDTOs
                    (
                        rm.Modules.Id,
                        rm.Modules.Name,
                        rm.Modules.Role,
                        rm.Modules.TargetUrl,
                        rm.Modules.IsActive

                        )).ToList();


                return Result<List<ModulesGetDTOs>>.Success(resultDTOs);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Getting modules by RoleId");
            }
        }

        public async Task<Result<List<ModulesGetDTOs>>> GetModulesByUserId(string userId)
        {
            try
            {
                //Retrives all the roles for the users
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                //Retrives modulesby using roles
                var modules = await _context.RoleModules
                    .Where(rm => roles.Contains(rm.RoleId))
                    .Select(rm => rm.Modules)
                    .ToListAsync();

                var resultDTOs = modules.Select(m => new ModulesGetDTOs(
                    m.Id,
                    m.Name,
                    m.Role,
                    m.TargetUrl,
                    m.IsActive
                    )).ToList();
                return Result<List<ModulesGetDTOs>>.Success(resultDTOs);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurd while geeting Modules by UserId");
            }
        }

        public Task<Result<ModulesGetDTOs>> GetModulesWithDetails(string moduleId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<ModulesGetDTOsWithUsers>>> GetNavigationMenuByUser(string userId)
        {
            try
            {
                // Retrieve all roles for the user
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                // Retrieve modules associated with these roles, including submodules and menus
                var modules = await _context.RoleModules
                    .Where(rm => roles.Contains(rm.RoleId))
                    .Select(rm => rm.Modules)
                    .Distinct()
                    .Include(m => m.SubModules)
                    .ThenInclude(sm => sm.Menu)
                    .ToListAsync();

                // Map the retrieved data to the DTO structure
                var resultDTO = modules.Select(m => new ModulesGetDTOsWithUsers(
                    m.Id,
                    m.Name,
                    m.Role,
                    m.TargetUrl,
                    m.IsActive,
                    m.SubModules.Select(sm => new SubModulesGetDTOsWithUsers(
                       sm.ModuleId,
                       sm.Id,
                       sm.Name,
                       sm.iconUrl,
                       sm.TargetUrl,
                       sm.Role,
                       sm.Rank,
                       sm.IsActive
                    )).ToList()))
                    .ToList();
                // m.SubModules.SelectMany(sm => sm.Menu.Select(menu => new MenuGetDTOs(
                //    menu.Id,
                //       menu.Name,
                //       menu.IconUrl,
                //       menu.TargetUrl,
                //       menu.Role,
                //       menu.Rank,
                //       menu.IsActive
                //))).ToList()))
                //.ToList();

                return Result<List<ModulesGetDTOsWithUsers>>.Success(resultDTO);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting the navigation menu", ex);
            }
        }

        public async Task<Result<GetModulesRoles>> RemoveModulesFromRoles(string roleId, IEnumerable<string> modulesId)
        {
            try
            {
                var rolesModulesRemoves = await _context.RoleModules
                    .Where(rm => rm.RoleId == roleId && modulesId.Contains(rm.ModuleId))
                    .ToListAsync();

                if (rolesModulesRemoves.Any())
                {
                    _context.RoleModules.RemoveRange(rolesModulesRemoves);
                    await _context.SaveChangesAsync();
                }

                var resultDTOs = new GetModulesRoles
                    (
                    roleId,
                    rolesModulesRemoves.Select(id => id.ToString()).ToList()
                    );

                return Result<GetModulesRoles>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Removing modules From Roles");
            }
        }

        public async Task<Result<List<bool>>> UpdateModulesFlagByModulesId(string userId, List<UpdateModulesFlag> updateModulesFlag)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (updateModulesFlag == null || !updateModulesFlag.Any())
                    {
                        return Result<List<bool>>.Failure("NotFound", "Please provide valid module IDs");
                    }

                    var rolesList = await _context.UserRoles
                        .Where(ur => ur.UserId == userId)
                        .Select(ur => ur.RoleId)
                        .ToListAsync();

                    if (!rolesList.Any())
                    {
                        return Result<List<bool>>.Failure("NotFound", "User doesnot have any role associated");
                    }


                    var modulesList = await _context.RoleModules
                        .Where(rm => rolesList.Contains(rm.RoleId))
                        .ToListAsync();

                    if (!modulesList.Any())
                    {
                        return Result<List<bool>>.Failure("NotFound", "No Modules found for associated roles");
                    }

                    var updateResults = new List<bool>();

                    foreach (var updateFlag in updateModulesFlag)
                    {
                        var checkModeslList = modulesList.FirstOrDefault(x => x.ModuleId == updateFlag.modulesId);
                        var moduleToUpdate = await _unitOfWork.Repository<Modules>().GetByIdAsync(checkModeslList.ModuleId);
                        if (moduleToUpdate is null)
                        {
                            return Result<List<bool>>.Failure("NotFound", "Menu are not found");
                        }


                        moduleToUpdate.IsActive = updateFlag.isActive;
                        await _unitOfWork.SaveChangesAsync();

                        updateResults.Add(moduleToUpdate.IsActive);

                    }
                    scope.Complete();
                    return Result<List<bool>>.Success(updateResults);

                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while updating");
                }
            }
        }
    }
}
