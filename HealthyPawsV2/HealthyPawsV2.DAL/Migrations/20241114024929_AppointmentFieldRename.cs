using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentFieldRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_AspNetUsers_veterinarioId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_veterinarioId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "veterinarioId",
                table: "Appointment");

            migrationBuilder.AlterColumn<string>(
                name: "vetId",
                table: "Appointment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_vetId",
                table: "Appointment",
                column: "vetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_AspNetUsers_vetId",
                table: "Appointment",
                column: "vetId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_AspNetUsers_vetId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_vetId",
                table: "Appointment");

            migrationBuilder.AlterColumn<string>(
                name: "vetId",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "veterinarioId",
                table: "Appointment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_veterinarioId",
                table: "Appointment",
                column: "veterinarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_AspNetUsers_veterinarioId",
                table: "Appointment",
                column: "veterinarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
