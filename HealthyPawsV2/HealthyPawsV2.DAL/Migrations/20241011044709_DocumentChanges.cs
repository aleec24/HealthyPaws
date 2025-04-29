using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DocumentChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Document_documentId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_documentId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "documentId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "appointmentId",
                table: "Document",
                newName: "petFileId");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Document",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Document_petFileId",
                table: "Document",
                column: "petFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_PetFile_petFileId",
                table: "Document",
                column: "petFileId",
                principalTable: "PetFile",
                principalColumn: "petFileId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_PetFile_petFileId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_petFileId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "petFileId",
                table: "Document",
                newName: "appointmentId");

            migrationBuilder.AddColumn<int>(
                name: "documentId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_documentId",
                table: "Appointment",
                column: "documentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Document_documentId",
                table: "Appointment",
                column: "documentId",
                principalTable: "Document",
                principalColumn: "documentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
