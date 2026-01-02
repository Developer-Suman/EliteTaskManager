using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddtaskDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Citizenships_CitizenshipId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Signatures_SignitureId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "CertificateDocuments");

            migrationBuilder.DropTable(
                name: "CertificateImages");

            migrationBuilder.DropTable(
                name: "CitizenshipImages");

            migrationBuilder.DropTable(
                name: "Nashu");

            migrationBuilder.DropTable(
                name: "Nijamatis");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Citizenships");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CitizenshipId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_SignitureId",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SignitureId",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CitizenshipId",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NickNames",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NickNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NickNameId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskStatus = table.Column<int>(type: "int", nullable: true),
                    ProjectDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskPriority = table.Column<int>(type: "int", nullable: true),
                    TaskReviewed = table.Column<int>(type: "int", nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoingLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskDetails_NickNames_NickNameId",
                        column: x => x.NickNameId,
                        principalTable: "NickNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskDetails_ProjectDetails_ProjectDetailsId",
                        column: x => x.ProjectDetailsId,
                        principalTable: "ProjectDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDetails_NickNameId",
                table: "TaskDetails",
                column: "NickNameId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDetails_ProjectDetailsId",
                table: "TaskDetails",
                column: "ProjectDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDetails");

            migrationBuilder.DropTable(
                name: "NickNames");

            migrationBuilder.DropTable(
                name: "ProjectDetails");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SignitureId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CitizenshipId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citizenships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CitizenshipNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDistrict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VdcOrMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizenships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nashu",
                columns: table => new
                {
                    NashuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemporaryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nashu", x => x.NashuId);
                });

            migrationBuilder.CreateTable(
                name: "Nijamatis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NijamatiName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nijamatis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nijamatis_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nijamatis_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Signatures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateDocuments",
                columns: table => new
                {
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateDocuments", x => new { x.DocumentsId, x.CertificateId });
                    table.ForeignKey(
                        name: "FK_CertificateDocuments_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateImgURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateImages_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizenshipImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CitizenshipId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenshipImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizenshipImages_Citizenships_CitizenshipId",
                        column: x => x.CitizenshipId,
                        principalTable: "Citizenships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CitizenshipId",
                table: "Documents",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SignitureId",
                table: "Documents",
                column: "SignitureId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateDocuments_CertificateId",
                table: "CertificateDocuments",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateImages_CertificateId",
                table: "CertificateImages",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenshipImages_CitizenshipId",
                table: "CitizenshipImages",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Nijamatis_DepartmentId",
                table: "Nijamatis",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Nijamatis_DocumentsId",
                table: "Nijamatis",
                column: "DocumentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Citizenships_CitizenshipId",
                table: "Documents",
                column: "CitizenshipId",
                principalTable: "Citizenships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Signatures_SignitureId",
                table: "Documents",
                column: "SignitureId",
                principalTable: "Signatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
