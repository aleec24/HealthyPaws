using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PetFileFieldRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetFile_AspNetUsers_duenoId",
                table: "PetFile");

            migrationBuilder.DropColumn(
                name: "idNumber",
                table: "PetFile");

            migrationBuilder.RenameColumn(
                name: "duenoId",
                table: "PetFile",
                newName: "ownerId");

            migrationBuilder.RenameIndex(
                name: "IX_PetFile_duenoId",
                table: "PetFile",
                newName: "IX_PetFile_ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetFile_AspNetUsers_ownerId",
                table: "PetFile",
                column: "ownerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetFile_AspNetUsers_ownerId",
                table: "PetFile");

            migrationBuilder.RenameColumn(
                name: "ownerId",
                table: "PetFile",
                newName: "duenoId");

            migrationBuilder.RenameIndex(
                name: "IX_PetFile_ownerId",
                table: "PetFile",
                newName: "IX_PetFile_duenoId");

            migrationBuilder.AddColumn<string>(
                name: "idNumber",
                table: "PetFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PetFile_AspNetUsers_duenoId",
                table: "PetFile",
                column: "duenoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
