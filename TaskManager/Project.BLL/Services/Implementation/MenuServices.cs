
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Menu;
using Project.BLL.Repository.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;


namespace Project.BLL.Services.Implementation
{
    public class MenuServices : IMenu
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public MenuServices(IMapper mapper, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = applicationDbContext;

        }
        public async Task<Result<MenuGetDTOs>> Add(MenuCreatesDTOs menuCreatesDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    if (menuCreatesDTOs.role is not null && !(await _roleManager.RoleExistsAsync(menuCreatesDTOs.role)))
                    {
                        return Result<MenuGetDTOs>.Failure("Role doesnot Exists");

                    }

                    var menuData = new Menu
                        (
                        newId,
                        menuCreatesDTOs.name,
                        menuCreatesDTOs.targetUrl,
                        menuCreatesDTOs.icon,
                        menuCreatesDTOs.role,
                        menuCreatesDTOs.submoduleId,
                        menuCreatesDTOs.rank,
                        menuCreatesDTOs.isActive
                        );
                    await _unitOfWork.Repository<Menu>().AddAsync(menuData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var resultDTOs = new MenuGetDTOs(
                        menuData.Id,
                        menuData.Name,
                        menuData.IconUrl,
                        menuData.TargetUrl,
                        menuData.Role,
                        menuData.Rank,
                        menuData.IsActive
                        );

                    return Result<MenuGetDTOs>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while addin");
                }
            }
        }

        public Task<Result<List<MenuGetDTOs>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<MenuGetDTOs>> GetById(string menuId)
        {
            try
            {
                var menu = await _unitOfWork.Repository<Menu>().GetByIdAsync(menuId);
                if (menu is null)
                {
                    return Result<MenuGetDTOs>.Failure("NotFound", "Menu Not Found");
                }

                var resultDTOs = new MenuGetDTOs(
                    menu.Id,
                    menu.Name,
                    menu.IconUrl,
                    menu.TargetUrl,
                    menu.Role,
                    menu.Rank,
                    menu.IsActive
                    );

                return Result<MenuGetDTOs>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Result<List<MenuGetDTOs>>> GetMenusBySubModulesId(string subModuleId)
        {
            try
            {
                var menus = await _unitOfWork.Repository<Menu>()
                    .GetAllAsync(
                    menu => subModuleId.Contains(menu.SubModuleId),
                    menu => menu.SubModules
                    );

                var resultDTOs = menus.Select(menu => new MenuGetDTOs
                (
                    menu.SubModules.Id,
                    menu.Name,
                    menu.IconUrl,
                    menu.TargetUrl,
                    menu.Role,
                    menu.Rank,
                    menu.IsActive
                 )).ToList();
                return Result<List<MenuGetDTOs>>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Menus By SubModulesId");
            }
        }

        public async Task<Result<MenuGetDTOs>> Remove(string menuId)
        {
            try
            {
                var menu = await _unitOfWork.Repository<Menu>().GetByIdAsync(menuId);
                if (menu is null)
                {
                    return Result<MenuGetDTOs>.Failure("NotFound", "Menu could not be found");
                }

                _unitOfWork.Repository<Menu>().Delete(menu);
                await _unitOfWork.SaveChangesAsync();

                var resultDTOs = new MenuGetDTOs
                    (
                    menu.Id,
                    menu.Name,
                    menu.IconUrl,
                    menu.TargetUrl,
                    menu.Role,
                    menu.Rank,
                    menu.IsActive
                    );

                return Result<MenuGetDTOs>.Success(resultDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Removing Menu");
            }
        }

        public async Task<Result<MenuGetDTOs>> Update(string menuId, MenuUpdateDTOs menuUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(menuId))
                    {
                        return Result<MenuGetDTOs>.Failure("NotFound", "Please provide valid menu id");
                    }

                    var menuToBeUpdated = await _unitOfWork.Repository<Menu>().GetByIdAsync(menuId);
                    if (menuToBeUpdated is null)
                    {
                        return Result<MenuGetDTOs>.Failure("NotFound", "Menu are not found");
                    }

                    _mapper.Map(menuUpdateDTOs, menuToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new MenuGetDTOs
                    (
                    menuToBeUpdated.Id,
                    menuToBeUpdated.Name,
                    menuToBeUpdated.IconUrl,
                    menuToBeUpdated.TargetUrl,
                    menuToBeUpdated.Role,
                    menuToBeUpdated.Rank,
                    menuToBeUpdated.IsActive
                    );

                    return Result<MenuGetDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while Updating");
                }
            }
        }
    }
}
