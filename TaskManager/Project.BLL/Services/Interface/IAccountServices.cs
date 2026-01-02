using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IAccountServices
    {
        Task<Result<RegistrationCreateDTOs>> RegisterUser (RegistrationCreateDTOs userModel, bool isAdminRegistration);
        Task<Result<ForgetPasswordDTOs>> ForgetPassword (ForgetPasswordDTOs forgetPasswordDTOs);

        Task<Result<RegistrationCreateDTOs>> VerifyOTP(int otp);
        Task<Result<TokenDTOs>> LoginUser (LogInDTOs logInDTOs);
        Task<Result<object>> LogoutUser(string userId);
        Task<Result<ChangePasswordDTOs>> ChangePassword(string userId,ChangePasswordDTOs changePasswordDTOs);
        Task<Result<string>> CreateRoles(string rolename);
        Task<Result<AssignRolesDTOs>> AssignRoles(AssignRolesDTOs assignRolesDTOs);
        Task<Result<TokenDTOs>> GetNewToken(TokenDTOs tokenDTOs);
        Task<Result<PagedResult<RoleDTOs>>> GetAllRoles(PaginationDTOs paginationDTOs, CancellationToken cancellationToken);
        Task<Result<PagedResult<UserDTOs>>> GetAllUsers(PaginationDTOs paginationDTOs, CancellationToken cancellationToken);
        Task<Result<UserDTOs>> GetByUserId(string userId, CancellationToken cancellationToken);
        Task<Result<ResetPasswordDTOs>> ResetPassword(ResetPasswordDTOs resetPasswordDTOs);
    }
}
