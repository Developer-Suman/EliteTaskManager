using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DLL.Models;
using Project.DLL.Models.Task;
using Project.DLL.Models.Task.SetUp;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        #region Task
        public DbSet<NickName> NickNames { get; set; }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }
        public DbSet<TaskDetails> TaskDetails { get; set; }
        #endregion


        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ControllerAction> ControllerActions { get; set; }

        public DbSet<UserControllerAction> UserControllerActions { get; set; }    


        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<VDC> Vdc { get; set; }



        #region Menu,Module,SubModule
        public DbSet<Modules> Modules { get; set; }
        public DbSet<SubModules> SubModules { get; set; }
        public DbSet<Menu> Menus { get; set; }

        public DbSet<RoleModule> RoleModules { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Task

            #region ProjectDetails and TaskDetails(1:m)
            builder.Entity<ProjectDetails>(entity =>
            {
                entity.HasMany(a => a.TaskDetails)
                      .WithOne(s => s.ProjectDetails)
                      .HasForeignKey(a => a.ProjectDetailsId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region NickNames and TaskDetails(1:m)
            builder.Entity<NickName>(entity =>
            {
                entity.HasMany(a => a.TaskDetails)
                      .WithOne(s => s.NickName)
                      .HasForeignKey(a => a.NickNameId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #endregion


            #region Branch and Department(1:m)
            builder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.BranchNameInEnglish).IsRequired();
                entity.Property(e=>e.BranchNameInNepali).IsRequired();
                entity.Property(e => e.BranchHeadNameInEnglish).IsRequired();
                entity.Property(e => e.BranchHeadNameInNepali).IsRequired();
                entity.Property(e=>e.IsActive).IsRequired();

                entity.HasMany(d => d.Departments)
                .WithOne(d => d.Branch)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Department and Branch(m:1)

            builder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DepartmentNameInNepali).IsRequired();
                entity.Property(e=>e.DepartmentNameInEnglish).IsRequired();
                entity.HasOne(a=>a.Branch)
                .WithMany(a=>a.Departments)
                .HasForeignKey(a=>a.BranchId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region UserData and Documents(1:m)
            builder.Entity<UserData>(entity =>
            {
                entity.HasMany(x => x.Documents)
                .WithOne(x => x.UserDatas)
                .HasForeignKey(x => x.UserDataId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Documents and UserDatas(m:1)
            builder.Entity<Documents>(entity =>
            {
                entity.HasOne(x=>x.UserDatas)
                .WithMany(x => x.Documents)
                .HasForeignKey(x=>x.UserDataId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region ApplicationUser and UserDatas(1:m)
            builder.Entity<ApplicationUsers>(entity =>
            {
                // Configure the one-to-many relationship from ApplicationUsers to UserDatas
                entity.HasMany(a => a.UserData)
                      .WithOne(b => b.ApplicationUser)
                      .HasForeignKey(b => b.UserId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });


            #endregion

            #region UserData and ApplicationUser(m:1)
            builder.Entity<UserData>(entity =>
            {
                // Configure the inverse relationship from UserData to ApplicationUsers
                entity.HasOne(b => b.ApplicationUser)
                      .WithMany(a => a.UserData)
                      .HasForeignKey(b => b.UserId)
                      .OnDelete(DeleteBehavior.Cascade);  // Ensure UserId is the foreign key in UserData pointing to ApplicationUser
            });

            #endregion

            #region ApplicationUser and ControllerAction(m:m)
            builder.Entity<UserControllerAction>()
                .HasKey(uc => new { uc.UserId, uc.ControllerActionId });


            builder.Entity<UserControllerAction>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.userControllerActions)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region ControllerAction and UserControllerActions

            builder.Entity<UserControllerAction>()
                .HasOne(uc => uc.ControllerActions)
                .WithMany(c => c.userControllerActions)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Menu and SubModule(m:1)
            builder.Entity<Menu>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SubModules)
                .WithMany(x => x.Menu)
                .HasForeignKey(x => x.SubModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region SubModule and Menu(1:m)
            builder.Entity<SubModules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Menu)
                .WithOne(x => x.SubModules)
                .HasForeignKey(x => x.SubModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion


            #region Module and SubModule(1:m)
            builder.Entity<Modules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.SubModules)
                .WithOne(x => x.Modules)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region SubModule and Module(m:1)
            builder.Entity<SubModules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Modules)
                .WithMany(x => x.SubModules)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion


            #region Modules and Roles(m:m)

            // Configure many-to-many relationship between roles and modules
            builder.Entity<RoleModule>()
                .HasKey(rm => new { rm.RoleId, rm.ModuleId });

            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Role)
                .WithMany()
                .HasForeignKey(rm => rm.RoleId);

            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Modules)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(rm => rm.ModuleId);

            #endregion

        }
    }
}
