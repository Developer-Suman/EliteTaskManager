using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.UserData;
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
    public class UserDataController : WebBaseController
    {
        private readonly IUserDataRepository _userDataRepository;

        public UserDataController(IUserDataRepository userDataRepository,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _userDataRepository =  userDataRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] CreateUserDataDTOs createUserDataDTOs, IFormFile imagefiles)
        {
            await GetCurrentUser();
            var saveUserDataResult = await _userDataRepository.SaveUserData(createUserDataDTOs, imagefiles, _currentUser!.Id);
            #region switch
            return saveUserDataResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Save), saveUserDataResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveUserDataResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            var deleteuserDataResult = await _userDataRepository.DeleteUserData(Id);

            #region switch
            return deleteuserDataResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteuserDataResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateDocuments([FromRoute] string Id, [FromForm] UpdateUserDataDTOs userDataDTOs, IFormFile imegeFiles)
        {
            var updateuserDataResult = await _userDataRepository.UpdateUserData(Id, userDataDTOs, imegeFiles);

            #region switch
            return updateuserDataResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateuserDataResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateuserDataResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getAluserData = await _userDataRepository.GetAllUserData(paginationDTOs, cancellationToken);


            #region switch
            return getAluserData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAluserData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAluserData.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _userDataRepository.GetUserDataById(Id, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


    }
}
