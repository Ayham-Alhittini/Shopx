using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class updateKeyForBrowseHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrowseHistories_AspNetUsers_CustomerId",
                table: "BrowseHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrowseHistories",
                table: "BrowseHistories");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "BrowseHistories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BrowseHistories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrowseHistories",
                table: "BrowseHistories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BrowseHistories_CustomerId",
                table: "BrowseHistories",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrowseHistories_AspNetUsers_CustomerId",
                table: "BrowseHistories",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrowseHistories_AspNetUsers_CustomerId",
                table: "BrowseHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrowseHistories",
                table: "BrowseHistories");

            migrationBuilder.DropIndex(
                name: "IX_BrowseHistories_CustomerId",
                table: "BrowseHistories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BrowseHistories");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "BrowseHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrowseHistories",
                table: "BrowseHistories",
                columns: new[] { "CustomerId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BrowseHistories_AspNetUsers_CustomerId",
                table: "BrowseHistories",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
