using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.DTOs.Pagination;
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
    public class DocumentsController : WebBaseController
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsController(IDocumentsRepository documentsRepository,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager): base(mapper,userManager,roleManager)
        {
            _documentsRepository = documentsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] DocumentsCreateDTOs documentsCreateDTOs)
        {
            await GetCurrentUser();
            var saveDocumentseResult = await _documentsRepository.SaveDocuments(documentsCreateDTOs, _currentUser!.Id);
            #region switch
            return saveDocumentseResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Save), saveDocumentseResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveDocumentseResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            var deleteDocumentsResult = await _documentsRepository.DeleteDocuments(Id);

            #region switch
            return deleteDocumentsResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteDocumentsResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> Update([FromRoute] string Id, [FromForm] DocumentsUpdateDTOs documentsUpdateDTOs)
        {
            var updateDocumentsResult = await _documentsRepository.UpdateDocuments(Id, documentsUpdateDTOs);

            #region switch
            return updateDocumentsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateDocumentsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateDocumentsResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getAllDocumentsData = await _documentsRepository.GetAll(paginationDTOs, cancellationToken);


            #region switch
            return getAllDocumentsData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllDocumentsData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllDocumentsData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _documentsRepository.GetById(Id, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


    }
}
