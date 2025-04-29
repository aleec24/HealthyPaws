using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    canton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    documentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointmentId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileType = table.Column<byte>(type: "tinyint", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.documentId);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    inventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    availableStock = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    provider = table.Column<double>(type: "float", nullable: false),
                    providerInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.inventoryId);
                });

            migrationBuilder.CreateTable(
                name: "PetTypes",
                columns: table => new
                {
                    petTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypes", x => x.petTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    surnames = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    lastConnection = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    phone1 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    phone2 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    phone3 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    addressId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Address_addressId",
                        column: x => x.addressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetBreeds",
                columns: table => new
                {
                    petBreedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    petTypeId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetBreeds", x => x.petBreedId);
                    table.ForeignKey(
                        name: "FK_PetBreeds_PetTypes_petTypeId",
                        column: x => x.petTypeId,
                        principalTable: "PetTypes",
                        principalColumn: "petTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogReports",
                columns: table => new
                {
                    LogReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorIdId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogReports", x => x.LogReportId);
                    table.ForeignKey(
                        name: "FK_LogReports_AspNetUsers_CreatorIdId",
                        column: x => x.CreatorIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PetFile",
                columns: table => new
                {
                    petFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    petBreedId = table.Column<int>(type: "int", nullable: false),
                    idNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    petTypeId = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vaccineHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    castration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    duenoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFile", x => x.petFileId);
                    table.ForeignKey(
                        name: "FK_PetFile_AspNetUsers_duenoId",
                        column: x => x.duenoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PetFile_PetBreeds_petBreedId",
                        column: x => x.petBreedId,
                        principalTable: "PetBreeds",
                        principalColumn: "petBreedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    petFile = table.Column<int>(type: "int", nullable: false),
                    vetId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ownerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    documentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    diagnostic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Additional = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    petIdpetFileId = table.Column<int>(type: "int", nullable: true),
                    veterinarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_AspNetUsers_ownerId",
                        column: x => x.ownerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_AspNetUsers_veterinarioId",
                        column: x => x.veterinarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointment_Document_documentId",
                        column: x => x.documentId,
                        principalTable: "Document",
                        principalColumn: "documentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_PetFile_petIdpetFileId",
                        column: x => x.petIdpetFileId,
                        principalTable: "PetFile",
                        principalColumn: "petFileId");
                });

            migrationBuilder.CreateTable(
                name: "AppointmentInventories",
                columns: table => new
                {
                    appointmentInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointmentId = table.Column<int>(type: "int", nullable: false),
                    inventoryID = table.Column<int>(type: "int", nullable: false),
                    dose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    measuredose = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentInventories", x => x.appointmentInventoryId);
                    table.ForeignKey(
                        name: "FK_AppointmentInventories_Appointment_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointment",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentInventories_Inventories_inventoryID",
                        column: x => x.inventoryID,
                        principalTable: "Inventories",
                        principalColumn: "inventoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_documentId",
                table: "Appointment",
                column: "documentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ownerId",
                table: "Appointment",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_petIdpetFileId",
                table: "Appointment",
                column: "petIdpetFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_veterinarioId",
                table: "Appointment",
                column: "veterinarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentInventories_appointmentId",
                table: "AppointmentInventories",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentInventories_inventoryID",
                table: "AppointmentInventories",
                column: "inventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_addressId",
                table: "AspNetUsers",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_LogReports_CreatorIdId",
                table: "LogReports",
                column: "CreatorIdId");

            migrationBuilder.CreateIndex(
                name: "IX_PetBreeds_petTypeId",
                table: "PetBreeds",
                column: "petTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFile_duenoId",
                table: "PetFile",
                column: "duenoId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFile_petBreedId",
                table: "PetFile",
                column: "petBreedId");
        }

        ///// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "AppointmentInventories");

        //    migrationBuilder.DropTable(
        //        name: "LogReports");

        //    migrationBuilder.DropTable(
        //        name: "Appointment");

        //    migrationBuilder.DropTable(
        //        name: "Inventories");

        //    migrationBuilder.DropTable(
        //        name: "Document");

        //    migrationBuilder.DropTable(
        //        name: "PetFile");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUsers");

        //    migrationBuilder.DropTable(
        //        name: "PetBreeds");

        //    migrationBuilder.DropTable(
        //        name: "Address");

        //    migrationBuilder.DropTable(
        //        name: "PetTypes");
        //}
    }
}
