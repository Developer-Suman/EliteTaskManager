using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Project.BLL.DTOs;
using Project.BLL.DTOs.Initialize;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Project.BLL.Services.Implementation
{
    public class InitializeRepository : IinitializeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DataSeeder _dataSeeder;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly Assembly _assembly;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public InitializeRepository(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,ApplicationDbContext context, DataSeeder dataSeeder, IAuthenticationRepository authenticationRepository)
        {
            _context = context;
            _dataSeeder = dataSeeder;
            _authenticationRepository = authenticationRepository;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _assembly = Assembly.GetExecutingAssembly();
        }

        public async Task<Result<string>> ControllerActionSeeder()
        {
            try
            {
                var controllerActionInfos = new List<ControllerAction>();
                // Get all action descriptors
                var actionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors.Items;

                // Extract controller names
                var controllerNames = actionDescriptors
                    .OfType<ControllerActionDescriptor>()
                    .Select(ad => ad.ControllerName)
                    .Distinct()
                    .ToList();

                foreach (var desciptor in actionDescriptors.OfType<ControllerActionDescriptor>())
                {
                    var controllerName = desciptor.ControllerName;
                    var actionName = desciptor.ActionName;

                    if(desciptor.ActionName.Contains("GetById"))
                    {

                    }

                    var controllerAction = new ControllerAction(
                       id: Guid.NewGuid().ToString(),  
                       controlerName: controllerName,
                       actionName: actionName
                    );

                    controllerActionInfos.Add(controllerAction);
                }


                // Save the list to your database
                _context.ControllerActions.AddRange(controllerActionInfos);
                await _context.SaveChangesAsync();
                return Result<string>.Success("Controller and Action are seeded Successfully");
            }
            catch (Exception ex)
            {
                // Log exception details (ex) for debugging
                return Result<string>.Failure($"An error occurred while seeding: {ex.Message}");
            }
        }



        public async Task<Result<InitializeDTOs>> InitializeAsync()
        {
            await _context.Database.MigrateAsync();
            var successMessage = await SeedAsync();

            return Result<InitializeDTOs>.Success(successMessage);
        }

        private async Task<InitializeDTOs> SeedAsync()
        {
            // Clear data from all tables dynamically
            var entityTypes = _context.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName();
                if (tableName != null)
                {
                    await _context.Database.ExecuteSqlRawAsync($"DELETE FROM [{tableName}]");
                }
            }

            // Insert initial data
            if (!_context.Branches.Any())
            {
                await _dataSeeder.Seed();

                var userName = "superadminuser";
                var email = "superadmin@gmail.com";
                var password = "Admin@123";
                var user = new ApplicationUsers { UserName = userName, Email = email };
                await _authenticationRepository.CreateUserAsync(user, password);
                await _authenticationRepository.AssignRoles(user, "superadmin");

                return new InitializeDTOs(userName, password, "Initialized Successfully");
            }

            return new InitializeDTOs(null!, null!, "Already Initialized");
        }
    }

}
