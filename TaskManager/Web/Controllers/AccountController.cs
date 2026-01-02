using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.WebUtilities;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System.Text;
using System.Text.Json;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AccountController : WebBaseController
    {
        private readonly IAccountServices _accountServices;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtProviders _jwtProviders;
        private readonly IOtpService _otpService;

        public AccountController(IOtpService otpService,IAccountServices accountServices, IAuthenticationRepository authenticationRepository,IJwtProviders jwtProviders, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _otpService = otpService;
            _accountServices = accountServices;
            _authenticationRepository = authenticationRepository;
            _jwtProviders = jwtProviders;      
        }

        #region LogIn
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTOs logInDTOs)
        {



            var logInResult = await _accountServices.LoginUser(logInDTOs);
        
            #region Switch Statement
            return logInResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(logInResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(logInResult.Errors),
                _ => BadRequest("Invalid Username and Password")
            };

            #endregion
        }

        #endregion

        #region Registration
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationCreateDTOs registrationCreateDTOs, bool isAdminRegistration)
        {
            var registrationResult = await _accountServices.RegisterUser(registrationCreateDTOs, isAdminRegistration);

            #region switch Statement
            return registrationResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Register), registrationResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = registrationResult.Message}),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(registrationResult.Errors),
                _ => BadRequest("Invalid Fields for Register User")
            };
            #endregion
        }
        #endregion


        #region VerifyToken
        [AllowAnonymous]
        [HttpPost("VerifyToken")]
        public async Task<IActionResult> VerifyToken(int otp)
        {
            var verifytokenResult = await _accountServices.VerifyOTP(otp);

            #region switch Statement
            return verifytokenResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(verifytokenResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(verifytokenResult.Errors),
                _ => BadRequest("Invalid Fields for VerifyToken")
            };
            #endregion
        }
        #endregion

        #region Create Roles
        [AllowAnonymous]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoles([FromQuery] string rolename)
        {
            var roleResult = await _accountServices.CreateRoles(rolename);
            #region switch Statement
            return roleResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(CreateRoles), roleResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = roleResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(roleResult.Errors),
                _ => BadRequest("Invalid rolename Fields")
            };
            #endregion
        }
        #endregion


        #region ForgetPassword
        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTOs forgetPasswordDTOs)
        {
            var forgetPasswordResult = await _accountServices.ForgetPassword(forgetPasswordDTOs);
            #region switch Statement
            return forgetPasswordResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(forgetPasswordResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = forgetPasswordResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(forgetPasswordResult.Errors),
                _ => BadRequest("Invalid email Fields")
            };
            #endregion
        }
        #endregion

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {

            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var resetPasswordInfo = new ResetPasswordDTOs(email, decodedToken,"");

        return Ok(resetPasswordInfo);
        }




        #region Reset Password
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordDTOs resetPasswordDTOs)
        {
            var passwordResetResult = await _accountServices.ResetPassword(resetPasswordDTOs);
            #region switch Statement
            return passwordResetResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(CreateRoles), passwordResetResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = passwordResetResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(passwordResetResult.Errors),
                _ => BadRequest("Invalid rolename Fields")
            };
            #endregion
        }
        #endregion

        #region Assign Roles
        [HttpPost("AssignRoles")]
        public async Task<IActionResult> AssignRoles([FromBody] AssignRolesDTOs assignRolesDTOs)
        {
            var assignRolesResult = await _accountServices.AssignRoles(assignRolesDTOs);
            #region switch Statement
            return assignRolesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(assignRolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignRolesResult.Errors),
                _ => BadRequest("Invalid rolename and userId Fields ")
            };
            #endregion
        }
        #endregion

        #region New RefreshToken
        [HttpPost("GetNewToken")]
        public async Task<IActionResult> GetNewToken([FromBody] TokenDTOs tokenDTOs)
        {
            var getNewTokenResult = await _accountServices.GetNewToken(tokenDTOs);

            #region Switch Statement
            return getNewTokenResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getNewTokenResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getNewTokenResult.Errors),
                _ => BadRequest("Invalid accesstoken and refreshtoken Fields ")
            };
            #endregion
        }
        #endregion


        #region GetAllUser
        [AllowAnonymous]
        [HttpGet("GetAllUser")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles = "departadmin, admin")]
        public async Task<IActionResult> GetAllUser([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {

            //var userRoles = GetCurrentUserRoles();
            var getAllUserResult =await _accountServices.GetAllUsers(paginationDTOs, cancellationToken);
            #region Switch Statement
            return getAllUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllUserResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }
        #endregion


        #region GetByUserId
        [HttpGet("GetByUserId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> GetByUserId(string Id, CancellationToken cancellationToken)
        {
            var getbyUserIdResult = await _accountServices.GetByUserId(Id, cancellationToken);

            #region Switch Statement
            return getbyUserIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getbyUserIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getbyUserIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }
        #endregion

        #region GetAllRoles
        [AllowAnonymous]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllUserRoles([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getAllUserRolesResult = await _accountServices.GetAllRoles(paginationDTOs, cancellationToken);
            #region Switch Statement
            return getAllUserRolesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllUserRolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllUserRolesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion

        }
        #endregion

        #region LogOutUser
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("LogOut")]
        public async Task<IActionResult> logOut()
        {
            await GetCurrentUser();
            var LogOutResult = await _accountServices.LogoutUser(_currentUser!.Id.ToString());
            #region Switch Statement
            return LogOutResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(LogOutResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(LogOutResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }

        #endregion

        #region ChangePassword
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTOs changePasswordDTOs)
        {
            await GetCurrentUser();
            var changePasswordResult = await _accountServices.ChangePassword(_currentUser!.Id.ToString(), changePasswordDTOs);
            #region Switch Statement
            return changePasswordResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(changePasswordResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(changePasswordResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }
        #endregion
    }
}
