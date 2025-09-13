using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHomePageToPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHomePage",
                table: "AppPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AppPages_IsHomePage",
                table: "AppPages",
                column: "IsHomePage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppPages_IsHomePage",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "IsHomePage",
                table: "AppPages");
        }
    }
}
