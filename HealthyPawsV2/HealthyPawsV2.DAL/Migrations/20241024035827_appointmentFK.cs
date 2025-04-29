using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class appointmentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Document_AppointmentId",
                table: "Document",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Appointment_AppointmentId",
                table: "Document",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Appointment_AppointmentId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_AppointmentId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Document");
        }
    }
}
