using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationRepository(IUnitOfWork unitOfWork ,UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
        }
        public async Task<IdentityResult> AssignRoles(ApplicationUsers user, string rolename)
        {
            return await _userManager.AddToRoleAsync(user, rolename);
        }

      

        public async Task<IdentityResult> ChangePassword(ApplicationUsers user,ChangePasswordDTOs changePasswordDTOs)
        {
            return await _userManager.ChangePasswordAsync(user, changePasswordDTOs.CurrentPassword, changePasswordDTOs.NewPassword);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUsers username, string password)
        {
            return await _userManager.CheckPasswordAsync(username, password);
        }

        public Task<bool> CheckRolesAsync(string role)
        {
            return _roleManager.RoleExistsAsync(role);
        }

        public async Task<IdentityResult> CreateRoles(string roles)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roles));
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUsers user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUsers> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<ApplicationUsers> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<ApplicationUsers> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUsers users)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(users);
        }

        public async Task<Result<PagedResult<RoleDTOs>>> GetAllRolesAsync(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Repository<IdentityRole>().GetAllAsyncWithPagination();
            var rolesPagedResult = await roles.AsNoTracking().ToPagedResultAsync(paginationDTOs.pageIndex, paginationDTOs.pageSize, paginationDTOs.IsPagination);

            var rolesDTOs = _mapper.Map<PagedResult<RoleDTOs>>(rolesPagedResult.Data);

            return Result<PagedResult<RoleDTOs>>.Success(rolesDTOs);
      
        }


        public async Task<Result<PagedResult<UserDTOs>>> GetAllUsersAsync(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {        
            var users = await _unitOfWork.Repository<ApplicationUsers>().GetAllAsyncWithPagination();
            var usersPagedResult = await users.AsNoTracking().ToPagedResultAsync(paginationDTOs.pageIndex, paginationDTOs.pageSize, paginationDTOs.IsPagination);

            var userDTOs = _mapper.Map<PagedResult<UserDTOs>>(usersPagedResult.Data);

            return Result<PagedResult<UserDTOs>>.Success(userDTOs);

        }

        public async Task<UserDTOs> GetById(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return default!;
            }
            var userDTOs = _mapper.Map<UserDTOs>(user);
            return userDTOs;
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUsers username)
        {
            return await _userManager.GetRolesAsync(username);

        }

        public async Task<IdentityResult> ResetPassword(ApplicationUsers users, ResetPasswordDTOs resetPasswordDTOs)
        {
            return await _userManager.ResetPasswordAsync(users, resetPasswordDTOs.token, resetPasswordDTOs.password);
        }

        public Task UpdateUserAsync(ApplicationUsers user)
        {
            return _userManager.UpdateAsync(user);
        }
    }
}
