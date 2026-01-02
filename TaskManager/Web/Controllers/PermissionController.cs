using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.ModulesFlags;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Reflection;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : WebBaseController
    {
        private readonly IModule _module;
        private readonly ISubModules _subModule;
        private readonly IMenu _menu;
        public PermissionController(ISubModules subModules, IMenu menu, IModule module, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _subModule = subModules;
            _menu = menu;
            _module = module;

        }

        [HttpPatch("UpdateModulesFlagByModulesId")]
        public async Task<IActionResult> UpdateModulesFlagByModulesId(string userId, [FromBody] List<UpdateModulesFlag> updateModulesFlag)
        {
            var updatemenuResult = await _module.UpdateModulesFlagByModulesId(userId,updateModulesFlag);
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
    }
}
