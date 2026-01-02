using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Menu;
using Project.BLL.DTOs.Modules;
using Project.BLL.DTOs.SubModules;
using Project.BLL.Services.Interface;
using Project.DLL.DbContext;
using Project.DLL.Models;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemManagementController : WebBaseController
    {
        private readonly IModule _module;
        private readonly ISubModules _subModule;
        private readonly IMenu _menu;

        public SystemManagementController(ISubModules subModules, IMenu menu,IModule module, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _subModule = subModules;
            _menu = menu;
            _module = module;
         
        }

        [HttpPost("add-module")]
        public async Task<IActionResult> AddModule([FromBody] ModulesCreateDTOs modulesCreateDTOs)
        {
            var savemoduleResult = await _module.Add(modulesCreateDTOs);
            #region switch
            return savemoduleResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddModule), savemoduleResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(savemoduleResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpPost("add-submodule")]
        public async Task<IActionResult> AddSubModule([FromBody] SubModulesCreateDTOs subModulesCreateDTOs)
        {
            var savesubmoduleResult = await _subModule.Add(subModulesCreateDTOs);
            #region switch
            return savesubmoduleResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSubModule), savesubmoduleResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(savesubmoduleResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpPost("add-menu")]
        public async Task<IActionResult> AddMenu([FromBody] MenuCreatesDTOs menuCreateDTOs)
        {
            var savemenuResult = await _menu.Add(menuCreateDTOs);
            #region switch
            return savemenuResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddMenu), savemenuResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(savemenuResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("DeleteMenu/{Id}")]
        public async Task<IActionResult> DeleteMenu([FromRoute] string Id)
        {
            var deleteMenuResult = await _menu.Remove(Id);
            #region switch
            return deleteMenuResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteMenuResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateMenu/{Id}")]
        public async Task<IActionResult> UpdateMenu([FromRoute] string Id, [FromBody] MenuUpdateDTOs menuUpdateDTOs)
        {
            var updatemenuResult = await _menu.Update(Id,menuUpdateDTOs);
            #region switch
            return updatemenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatemenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatemenuResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }
        [HttpGet("GetModulesByUserId/{Id}")]
        public async Task<IActionResult> GetModulesByUserId([FromRoute] string Id)
        {
            var getmoduleResult = await _module.GetModulesByUserId(Id);
            #region switch
            return getmoduleResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getmoduleResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getmoduleResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpGet("GetMenuWithDetails/{Id}")]
        public async Task<IActionResult> GetMenuWithDetails([FromRoute] string Id)
        {
            var getmenuResult = await _menu.GetById(Id);
            #region switch
            return getmenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getmenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getmenuResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }

        [HttpGet("GetMenusBySubModulesId/{Id}")]
        public async Task<IActionResult> GetMenusBySubModulesId([FromRoute] string Id)
        {
            var getListmenuResult = await _menu.GetMenusBySubModulesId(Id);
            #region switch
            return getListmenuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getListmenuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getListmenuResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpGet("GetModulesByRolesId/{Id}")]
        public async Task<IActionResult> GetModulesByRolesId([FromRoute] string Id)
        {
            var getModulesByRoleIdResult = await _module.GetModulesByRolesId(Id);
            #region switch
            return getModulesByRoleIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getModulesByRoleIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getModulesByRoleIdResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpPost("AssignModuleToRole/{roleId}")]
        public async Task<IActionResult> AssignModuleToRole([FromRoute] string roleId, [FromBody] IEnumerable<string> moduleIds)
        {
            var saveassignModelResult = await _module.AssignModulesToRoles(roleId, moduleIds);
            #region switch
            return saveassignModelResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignModuleToRole), saveassignModelResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveassignModelResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("RemoveModulesFromRole/{roleId}")]
        public async Task<IActionResult> RemoveModulesFromRole([FromRoute] string roleId, [FromBody] IEnumerable<string> moduleIds)
        {
            var removeassignModelResult = await _module.RemoveModulesFromRoles(roleId, moduleIds);
            #region switch
            return removeassignModelResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignModuleToRole), removeassignModelResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(removeassignModelResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpPost("GetNavigationMenuByUser/{userId}")]
        public async Task<IActionResult> GetNavigationMenuByUser([FromRoute] string userId)
        {
            var userNavigationResult = await _module.GetNavigationMenuByUser(userId);
            #region switch
            return userNavigationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(userNavigationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(userNavigationResult.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }
    }
}
