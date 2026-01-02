using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Authentication;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitiallizeController : WebBaseController
    {
        private readonly IinitializeRepository _initializeRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        public InitiallizeController(IAuthenticationRepository authenticationRepository , IinitializeRepository iinitializeRepository ,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _initializeRepository = iinitializeRepository;
            _authenticationRepository = authenticationRepository;
        }

        #region Initiallize
        [HttpPost()]
        public async Task<IActionResult> Initiallize()
        {
            var initiallizeResult = await _initializeRepository.InitializeAsync();

            #region switch
            return initiallizeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(initiallizeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(initiallizeResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }
        #endregion
    }
}
