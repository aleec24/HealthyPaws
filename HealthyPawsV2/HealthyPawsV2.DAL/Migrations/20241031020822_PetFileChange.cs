using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PetFileChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_PetFile_petIdpetFileId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_petIdpetFileId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "petIdpetFileId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "petFile",
                table: "Appointment",
                newName: "petFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_petFileId",
                table: "Appointment",
                column: "petFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_PetFile_petFileId",
                table: "Appointment",
                column: "petFileId",
                principalTable: "PetFile",
                principalColumn: "petFileId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_PetFile_petFileId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_petFileId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "petFileId",
                table: "Appointment",
                newName: "petFile");

            migrationBuilder.AddColumn<int>(
                name: "petIdpetFileId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_petIdpetFileId",
                table: "Appointment",
                column: "petIdpetFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_PetFile_petIdpetFileId",
                table: "Appointment",
                column: "petIdpetFileId",
                principalTable: "PetFile",
                principalColumn: "petFileId");
        }
    }
}
