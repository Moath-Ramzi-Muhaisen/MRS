using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class updatecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Users_UserId",
                table: "RequestHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RequestHistories",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestHistories_UserId",
                table: "RequestHistories",
                newName: "IX_RequestHistories_EmployeeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TechnicianId",
                table: "TechnicianCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewStatus",
                table: "RequestHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianNotes",
                table: "RequestDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Users_EmployeeId",
                table: "RequestHistories",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Users_EmployeeId",
                table: "RequestHistories");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "RequestHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestHistories_EmployeeId",
                table: "RequestHistories",
                newName: "IX_RequestHistories_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TechnicianId",
                table: "TechnicianCategories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "NewStatus",
                table: "RequestHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianNotes",
                table: "RequestDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Users_UserId",
                table: "RequestHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
