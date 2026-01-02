using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DLL.Models;
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

      
        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Nashu> Nashu { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }


        public DbSet<Signature> Signatures { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Citizenship> Citizenships { get; set; }

        public DbSet<UserData> UserDatas { get; set; }

        public DbSet<CertificateImages> CertificateImages { get; set; }

        public DbSet<CitizenshipImages> CitizenshipImages { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Nijamati> Nijamatis { get; set; }


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

            #region Signature and Documents(1:m)
            builder.Entity<Signature>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SignatureURL).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasMany(s => s.Documents)
                      .WithOne(d => d.Signature)
                      .HasForeignKey(d => d.SignitureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Documents and Signiture(m:1)
            builder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DocumentType).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedBy).IsRequired(false);

                entity.HasOne(d => d.Signature)
                      .WithMany(s => s.Documents)
                      .HasForeignKey(d => d.SignitureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
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

            #region Citizenship and Documents(1:m)
            builder.Entity<Citizenship>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(x => x.Documents)
                .WithOne(x => x.Citizenship)
                .HasForeignKey(x => x.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion

            #region Documents and Citizenship(m:1)
            builder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(s=>s.Citizenship)
                .WithMany(s => s.Documents)
                .HasForeignKey(x=>x.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Certificate and CertificateImages(1:m)
            builder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Grade).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired(false);
                entity.Property(e => e.Type).IsRequired(false);
                entity.Property(e => e.Board).IsRequired();

                entity.HasMany(d => d.CertificateImages)
                      .WithOne(e => e.Certificate)
                      .HasForeignKey(d => d.CertificateId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region CertificateImages and Certificate(m:1)
            builder.Entity<CertificateImages>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CertificateImgURL).IsRequired();
                entity.Property(e => e.CertificateId).IsRequired();

                entity.HasOne(e => e.Certificate)
                      .WithMany(e => e.CertificateImages)
                      .HasForeignKey(e => e.CertificateId); // Corrected foreign key configuration
            });
            #endregion

            #region Citizenship and CitizenshipImages(1:m)
            builder.Entity<Citizenship>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IssuedDate).IsRequired();
                entity.Property(e => e.IssuedDistrict).IsRequired();
                entity.Property(e => e.VdcOrMunicipality).IsRequired();
                entity.Property(e => e.WardNumber).IsRequired();
                entity.Property(e => e.DOB).IsRequired();
                entity.Property(e => e.CitizenshipNumber).IsRequired();


                entity.HasMany(d => d.CitizenshipImages)
                .WithOne(e => e.Citizenship)
                .HasForeignKey(d => d.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region CitizenshipImages and dCitizenship(m:1)
            builder.Entity<CitizenshipImages>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl);
                entity.Property(e => e.CreatedAt);
                entity.Property(e => e.CitizenshipId);


                entity.HasOne(e => e.Citizenship)
                .WithMany(e => e.CitizenshipImages)
                .HasForeignKey(e => e.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Certificate and Documents(m:m)
            builder.Entity<CertificateDocuments>()
                .HasKey(ud => new { ud.DocumentsId, ud.CertificateId });

            builder.Entity<CertificateDocuments>()
                .HasOne(e => e.Certificate)
                .WithMany(e => e.CertificateDocuments)
                .HasForeignKey(ud => ud.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CertificateDocuments>()
                .HasOne(e => e.Documents)
                .WithMany(e => e.certificateDocuments)
                .HasForeignKey(ud => ud.DocumentsId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Nijamati and Department(1:m)

            builder.Entity<Nijamati>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NijamatiName).IsRequired();

                entity.HasOne(x=>x.Department)
                .WithMany(x=>x.Nijamati)
                .HasForeignKey(x=>x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Department and Nijamati(m:1)
            builder.Entity<Department>(entity =>
            {
                entity.HasMany(x=>x.Nijamati)
                .WithOne(x=>x.Department)
                .HasForeignKey(x=>x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Nijamati and Documents(m:1)
            builder.Entity<Nijamati>(entity =>
            {
                entity.HasOne(a=>a.Documents)
                .WithMany(x => x.Nijamatis)
                .HasForeignKey(d => d.DocumentsId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Documents and Nijamati(1:m)
            builder.Entity<Documents>(entity =>
            {
                entity.HasMany(x=>x.Nijamatis)
                .WithOne(x=>x.Documents)
                .HasForeignKey(f=>f.DocumentsId)
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



            #region Citizenship and DocumentsImages(1:m)
            builder.Entity<Citizenship>()
                .HasMany(c => c.CitizenshipImages)
                .WithOne(cd => cd.Citizenship)
                .HasForeignKey(x => x.CitizenshipId)
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
