using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Pages");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RolePages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RolePages_RoleId",
                table: "RolePages",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePages_Roles_RoleId",
                table: "RolePages",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePages_Roles_RoleId",
                table: "RolePages");

            migrationBuilder.DropIndex(
                name: "IX_RolePages_RoleId",
                table: "RolePages");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RolePages");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
