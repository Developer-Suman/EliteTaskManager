using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Project.BLL.Cryptographys;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Department;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Interface;
using Project.BLL.Validator;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using Project.DLL.Static.Cache;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Transactions;
using Z.EntityFramework.Extensions.Core.SchemaObjectModel;


namespace Project.BLL.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly IMapper _mapper;
        private readonly IJwtProviders _jwtProviders;
        private readonly IConfiguration _config;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICryptography _cryptography;





        public AccountServices( ICryptography cryptography, IHttpContextAccessor httpContextAccessor, IMapper mapper, IJwtProviders jwtProviders, IConfiguration configuration, IAuthenticationRepository authenticationRepository, IMemoryCacheRepository memoryCacheRepository)
        {
            _mapper = mapper;
            _cryptography = cryptography;
            _jwtProviders = jwtProviders;
            _config = configuration;
            _authenticationRepository = authenticationRepository;
            _memoryCacheRepository = memoryCacheRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<Result<TokenDTOs>> LoginUser(LogInDTOs logInDTOs)
        {
            try
            {
                #region Validation
                var validationError = LogInValidator.LogInValidate(logInDTOs);
                if (validationError.Any())
                {
                    return Result<TokenDTOs>.Failure(validationError.ToArray());
                }

                #endregion
                var user = await _authenticationRepository.FindByEmailAsync(logInDTOs.Email);
                if (user == null)
                {
                    return Result<TokenDTOs>.Failure("Unauthorized", "Invalid Credentials");
                }
                if (!await _authenticationRepository.CheckPasswordAsync(user, logInDTOs.Password))
                {
                    return Result<TokenDTOs>.Failure("Unauthorized", "Invalid Password");
                }

                var roles = await _authenticationRepository.GetRolesAsync(user);
                if (roles is null)
                {
                    return Result<TokenDTOs>.Failure("NotFound", "Roles are not found");
                }
                string token = _jwtProviders.Generate(user, roles);

                string refreshToken = _jwtProviders.GenerateRefreshToken();
                user.RefreshToken = refreshToken;

                _ = int.TryParse(_config["Jwt:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _authenticationRepository.UpdateUserAsync(user);

                var logInToken = new TokenDTOs(token, refreshToken);
                return Result<TokenDTOs>.Success(logInToken);
            }

            catch (Exception ex)
            {
                throw new Exception("Something went Wrong while logging");
            }
        }

        public async Task<Result<RegistrationCreateDTOs>> RegisterUser(RegistrationCreateDTOs userModel, bool isAdminRegistration)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userExists = await _authenticationRepository.FindByNameAsync(userModel.Username);
                    if (userExists is not null)
                    {
                        return Result<RegistrationCreateDTOs>.Failure("Conflict", "User Already Exists");

                    }

                    var emailExists = await _authenticationRepository.FindByEmailAsync(userModel.Email);
                    if (emailExists is not null)
                    {
                        return Result<RegistrationCreateDTOs>.Failure("Conflict", "Email Already Exists");
                    }

                    var user = _mapper.Map<ApplicationUsers>(userModel);


                    if (isAdminRegistration)
                    {
                        var result = await _authenticationRepository.CreateUserAsync(user, userModel.Password);

                        if (!result.Succeeded)
                        {
                            scope.Dispose();
                            return Result<RegistrationCreateDTOs>.Failure("Conflict", "User Creation Failed");
                        }


                        //Add User to Desired Role
                        if (!string.IsNullOrEmpty(userModel.Role))
                        {
                            await _authenticationRepository.AssignRoles(user, userModel.Role);
                        }

                        //If Everything succeed Commit the transaction
                        scope.Complete();

                        var userDisplay = _mapper.Map<RegistrationCreateDTOs>(userModel);

                        return Result<RegistrationCreateDTOs>.Success(userDisplay);


                    }
                    else
                    {
                        byte[] key = await _cryptography.GenerateSecureKey();
                        byte[] iv = await _cryptography.GenerateSecureIV();

                        var password = userModel.Password;
                        string encryptedPassword = await _cryptography.Encrypt(password, key, iv);



                        var otp = new Random().Next(100000, 999999);
                        var otpService = _httpContextAccessor.HttpContext!.RequestServices.GetService<IOtpService>();
                        await otpService!.SendOtpAsync(userModel.Email, otp);
                        _httpContextAccessor.HttpContext.Session.SetString("Username", userModel.Username);
                        _httpContextAccessor.HttpContext.Session.SetString("UserEmail", userModel.Email);
                        _httpContextAccessor.HttpContext.Session.SetInt32("OTP", otp);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserEncryptedPassword", encryptedPassword);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserKey", Convert.ToBase64String(key));
                        _httpContextAccessor.HttpContext?.Session.SetString("UserIV", Convert.ToBase64String(iv));

                        //If Everything succeed Commit the transaction
                        scope.Complete();

                        return Result<RegistrationCreateDTOs>.Success("OTP Sent Successfully");

                    }

                }
                catch (Exception ex)
                {

                    throw new Exception("Something went wrong during user creation");
                }
            }
        }

        public async Task<Result<string>> CreateRoles(string rolename)
        {
            try
            {
                var roleExists = await _authenticationRepository.CheckRolesAsync(rolename);
                if (roleExists)
                {
                    return Result<string>.Failure("Conflict", "Roles Already Exists");
                }
                var result = await _authenticationRepository.CreateRoles(rolename);
                return Result<string>.Success($"Roles {rolename} successfully created");

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create Role");
            }
        }

        public async Task<Result<AssignRolesDTOs>> AssignRoles(AssignRolesDTOs assignRolesDTOs)
        {
            try
            {
                var user = await _authenticationRepository.FindByIdAsync(assignRolesDTOs.UserId);
                if (user is null)
                {
                    return Result<AssignRolesDTOs>.Failure("NotFound", "User not Found to assign Roles");
                }
                var result = await _authenticationRepository.AssignRoles(user, assignRolesDTOs.RoleName);
                return Result<AssignRolesDTOs>.Success(assignRolesDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Assign Roles");
            }
        }

        public async Task<Result<TokenDTOs>> GetNewToken(TokenDTOs tokenDTOs)
        {
            try
            {
                var principal = _jwtProviders.GetPrincipalFromExpiredToken(tokenDTOs.Token);
                if (principal is null)
                {
                    return Result<TokenDTOs>.Failure("Unauthorized", "Invalid Token");
                }

                string username = principal.Identity!.Name!;
                if (username is null)
                {
                    return Result<TokenDTOs>.Failure("Unauthorized", "Invalid Token");
                }

                var user = await _authenticationRepository.FindByNameAsync(username);

                if (user is null || user.RefreshToken != tokenDTOs.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return Result<TokenDTOs>.Failure("Unauthorized", "Invalid Access Token and refresh Token");
                }

                var roles = await _authenticationRepository.GetRolesAsync(user);
                var newToken = _jwtProviders.Generate(user, roles);

                var newRefreshToken = _jwtProviders.GenerateRefreshToken();
                user.RefreshToken = newRefreshToken;
                await _authenticationRepository.UpdateUserAsync(user);

                tokenDTOs = new TokenDTOs(newToken, newRefreshToken);

                //If u want to maintain immutable but need to update specific properties
                //tokenDTOs = tokenDTOs with { Token = newToken, RefreshToken =  newRefreshToken };

                //tokenDTOs.Token = newToken;
                //tokenDTOs.RefreshToken = newRefreshToken;

                return Result<TokenDTOs>.Success(tokenDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create New RefreshToken");
            }
        }

        public async Task<Result<PagedResult<UserDTOs>>> GetAllUsers(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = CacheKeys.User;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<UserDTOs>>(cacheKey);

                if (cacheData is not null)
                {
                    return Result<PagedResult<UserDTOs>>.Success(cacheData);
                }

                var allUser = await _authenticationRepository.GetAllUsersAsync(paginationDTOs, cancellationToken);
                var userDataDTOs = _mapper.Map<PagedResult<UserDTOs>>(allUser.Data);



                if (allUser is null)
                {
                    return Result<PagedResult<UserDTOs>>.Failure("NotFound", "Users are Not Found");
                }

                await _memoryCacheRepository.SetAsync(cacheKey, allUser, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<UserDTOs>>.Success(userDataDTOs);

            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch all the users");
            }
        }

        public async Task<Result<UserDTOs>> GetByUserId(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetByUserId{userId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<UserDTOs>(cacheKey);

                if (cacheData is not null)
                {
                    return Result<UserDTOs>.Success(cacheData);
                }
                var user = await _authenticationRepository.GetById(userId, cancellationToken);
                if (user is null)
                {
                    return Result<UserDTOs>.Failure("NotFound", "User are Not Found");
                }

                await _memoryCacheRepository.SetAsync(cacheKey, user, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);
                return Result<UserDTOs>.Success(user);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get User By UserId");
            }
        }

        public async Task<Result<PagedResult<RoleDTOs>>> GetAllRoles(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _authenticationRepository.GetAllRolesAsync(paginationDTOs, cancellationToken);
                if (roles is null)
                {
                    return Result<PagedResult<RoleDTOs>>.Failure("NotFound", "Roles are not Found");
                }

                var rolesList = _mapper.Map<PagedResult<RoleDTOs>>(roles.Data);
                return Result<PagedResult<RoleDTOs>>.Success(rolesList);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get All Roles");
            }
        }

        public async Task<Result<object>> LogoutUser(string userId)
        {
            try
            {
                var user = await _authenticationRepository.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result<object>.Failure("NotFound", "User is not Found");
                }
                user.RefreshToken = null;
                await _authenticationRepository.UpdateUserAsync(user);
                return Result<object>.Success(true);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to LogOut User");
            }
        }

        public async Task<Result<ChangePasswordDTOs>> ChangePassword(string userId, ChangePasswordDTOs changePasswordDTOs)
        {
            try
            {
                var user = await _authenticationRepository.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result<ChangePasswordDTOs>.Failure("NotFound", "User is not Found");
                }
                var changePasswordResult = await _authenticationRepository.ChangePassword(user, changePasswordDTOs);
                var changePassword = _mapper.Map<ChangePasswordDTOs>(changePasswordResult);
                return Result<ChangePasswordDTOs>.Success(changePassword);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to change Password");
            }
        }

        public async Task<Result<RegistrationCreateDTOs>> VerifyOTP(int otp)
        {
            try
            {
                var keyBase64 = _httpContextAccessor.HttpContext?.Session.GetString("UserKey");
                var ivBase64 = _httpContextAccessor.HttpContext?.Session.GetString("UserIV");


                if (keyBase64 == null || ivBase64 == null)
                {
                    return Result<RegistrationCreateDTOs>.Failure("Conflict", "Key or IV not found in session.");
                }

                byte[] key = Convert.FromBase64String(keyBase64);
                byte[] iv = Convert.FromBase64String(ivBase64);

                var email = _httpContextAccessor.HttpContext!.Session.GetString("UserEmail");
                var username = _httpContextAccessor.HttpContext!.Session.GetString("Username");
                var expectedOTP = _httpContextAccessor.HttpContext!.Session.GetInt32("OTP");
                var password = _httpContextAccessor.HttpContext?.Session.GetString("UserEncryptedPassword");


                var otpServices = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOtpService>();
                if (email is null || expectedOTP is null)
                {
                    return Result<RegistrationCreateDTOs>.Failure("Conflict", "OTP session is expired or invalid");
                }

                var decryptedPassword = await _cryptography.Decrypt(password, key, iv);

                string defaultRole = "User";
                var userModel = new RegistrationCreateDTOs(username, email, decryptedPassword, defaultRole);
                var user = _mapper.Map<ApplicationUsers>(userModel);

                if (otp.Equals(expectedOTP))
                {
                    var registrationResult = await _authenticationRepository.CreateUserAsync(user, decryptedPassword);
                    if (!registrationResult.Succeeded)
                    {
                        //scope.Dispose();
                        return Result<RegistrationCreateDTOs>.Failure("Conflict", "User Creation Failed");
                    }
                }
                else
                {
                    return Result<RegistrationCreateDTOs>.Failure("Unauthorized", "Invalid Token");
                }

                await _authenticationRepository.AssignRoles(user, defaultRole);
                var userDisplay = _mapper.Map<RegistrationCreateDTOs>(userModel) with { Role = defaultRole };


                _httpContextAccessor.HttpContext.Session.Clear();
                // Return the result with the mapped user data
                return Result<RegistrationCreateDTOs>.Success(userDisplay);
               
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Verify OTP");
            }
        }

        public async Task<Result<ResetPasswordDTOs>> ResetPassword(ResetPasswordDTOs resetPasswordDTOs)
        {
            try
            {

                var decodeByte = WebEncoders.Base64UrlDecode(resetPasswordDTOs.token);
                var decodedToken = Encoding.UTF8.GetString(decodeByte);


                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(resetPasswordDTOs.token);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return Result<ResetPasswordDTOs>.Failure("Unauthorized", "Token has expired.");
                }

                var user = await _authenticationRepository.FindByEmailAsync(resetPasswordDTOs.email);
                if(user is null)
                {
                    return Result<ResetPasswordDTOs>.Failure("NotFound", "User maybe not registered yet!");
                }

                var result = await _authenticationRepository.ResetPassword(user, resetPasswordDTOs);
                if (!result.Succeeded)
                {
                    return Result<ResetPasswordDTOs>.Failure("NotFound", "User send wrong credentials");

                }
                return Result<ResetPasswordDTOs>.Success("Password reset successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Reset password"+ ex.Message);
            }
        }

        public async Task<Result<ForgetPasswordDTOs>> ForgetPassword(ForgetPasswordDTOs forgetPasswordDTOs)
        {
            try
            {
                var user = await _authenticationRepository.FindByEmailAsync(forgetPasswordDTOs.email);
                if(user is null)
                {
                    return Result<ForgetPasswordDTOs>.Failure("NotFound","User Not Found");
                }

                var token = await _authenticationRepository.GeneratePasswordResetTokenAsync(user);

                var encodedToken = await _cryptography.Base64UrlEncoder(token);


                var request = _httpContextAccessor.HttpContext?.Request;
       

                var scheme = request.Scheme; // "http" or "https"
                var host = request.Host; // e.g., "yourdomain.com"


                var callBackUrl = $"{scheme}://{host}/Account/ResetPassword?token={encodedToken}&email={forgetPasswordDTOs.email}";
       

                if (callBackUrl is null)
                {
                    return Result<ForgetPasswordDTOs>.Failure("NotFound", "Failed to generate the reset password link");
                }

                var otpServices = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IOtpService>();
    
                var emailServices = _httpContextAccessor.HttpContext!.RequestServices.GetService<IEmailServices>();
                await emailServices.SendEmailAsync(forgetPasswordDTOs.email!, "Reset Password", $"Reset your password by <a href='{callBackUrl}'>Click here</a>.");

                return Result<ForgetPasswordDTOs>.Success("Successfully sent Link url");



            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while forgetting password");
            }
        }
    }
}
