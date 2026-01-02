using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Department;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    public class DepartmentController : WebBaseController
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _departmentRepository  = departmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DepartmentCreateDTOs departmentCreateDTOs)
        {
            var saveDepartmentResult = await _departmentRepository.SaveDepartment(departmentCreateDTOs);
            #region switch
            return saveDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Save), saveDepartmentResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveDepartmentResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var deleteDepartmentResult = await _departmentRepository.DeleteDepartment(Id, cancellationToken);

            #region switch
            return deleteDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteDepartmentResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> Update([FromRoute] string Id, [FromBody] DepartmentUpdateDTOs departmentUpdateDTOs)
        {
            var updateDepartmentResult = await _departmentRepository.UpdateDepartment(Id, departmentUpdateDTOs);

            #region switch
            return updateDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateDepartmentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateDepartmentResult.Errors),
                _ => BadRequest("Invalid DepartmentId")
            };
            #endregion
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getAlldepartmenthData = await _departmentRepository.GetAll(paginationDTOs, cancellationToken);


            #region switch
            return getAlldepartmenthData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAlldepartmenthData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAlldepartmenthData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _departmentRepository.GetById(Id, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }
    }
}
