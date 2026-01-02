using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Project.BLL.DTOs.Branch;
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
    public class BranchController : WebBaseController
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _branchRepository = branchRepository;
        }

        [HttpPost]
        [EnableRateLimiting("FixedWindowPolicy")]
        public async Task<IActionResult> Save([FromBody] BranchCreateDTOs branchCreateDTOs)
        {
            var saveBranchResult = await _branchRepository.SaveBranch(branchCreateDTOs);
            #region switch
            return saveBranchResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Save), saveBranchResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveBranchResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var deleteBranchResult = await _branchRepository.DeleteBranch(Id, cancellationToken);

            #region switch
            return deleteBranchResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteBranchResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> Update([FromRoute] string Id, [FromBody] BranchUpdateDTOs branchUpdateDTOs)
        {
            var updateBranchResult = await _branchRepository.UpdateBranch(Id, branchUpdateDTOs);

            #region switch
            return updateBranchResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateBranchResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBranchResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getAllbranchData = await _branchRepository.GetAll(paginationDTOs, cancellationToken);


            #region switch
            return getAllbranchData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllbranchData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllbranchData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _branchRepository.GetById(Id, cancellationToken);

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
