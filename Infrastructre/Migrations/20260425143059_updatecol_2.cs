using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class updatecol_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RequestHistories",
                newName: "ChangedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangedAt",
                table: "RequestHistories",
                newName: "CreatedAt");
        }
    }
}
