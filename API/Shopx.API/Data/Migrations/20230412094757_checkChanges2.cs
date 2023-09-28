using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class checkChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackgroundPhoto_AspNetUsers_AppUserId",
                table: "BackgroundPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BackgroundPhoto",
                table: "BackgroundPhoto");

            migrationBuilder.RenameTable(
                name: "BackgroundPhoto",
                newName: "Backgrounds");

            migrationBuilder.RenameIndex(
                name: "IX_BackgroundPhoto_AppUserId",
                table: "Backgrounds",
                newName: "IX_Backgrounds_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Backgrounds",
                table: "Backgrounds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Backgrounds_AspNetUsers_AppUserId",
                table: "Backgrounds",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Backgrounds_AspNetUsers_AppUserId",
                table: "Backgrounds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Backgrounds",
                table: "Backgrounds");

            migrationBuilder.RenameTable(
                name: "Backgrounds",
                newName: "BackgroundPhoto");

            migrationBuilder.RenameIndex(
                name: "IX_Backgrounds_AppUserId",
                table: "BackgroundPhoto",
                newName: "IX_BackgroundPhoto_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BackgroundPhoto",
                table: "BackgroundPhoto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BackgroundPhoto_AspNetUsers_AppUserId",
                table: "BackgroundPhoto",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
