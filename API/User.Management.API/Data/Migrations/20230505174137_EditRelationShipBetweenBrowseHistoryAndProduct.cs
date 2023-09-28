using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class EditRelationShipBetweenBrowseHistoryAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BrowseHistories_ProductId",
                table: "BrowseHistories");

            migrationBuilder.CreateIndex(
                name: "IX_BrowseHistories_ProductId",
                table: "BrowseHistories",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BrowseHistories_ProductId",
                table: "BrowseHistories");

            migrationBuilder.CreateIndex(
                name: "IX_BrowseHistories_ProductId",
                table: "BrowseHistories",
                column: "ProductId",
                unique: true);
        }
    }
}
