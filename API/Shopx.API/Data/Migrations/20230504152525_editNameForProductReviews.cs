using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class editNameForProductReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Backgrounds_CustomerBackgroundPhotoId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "CustomerKnownAs",
                table: "ProductReviews",
                newName: "KnownAs");

            migrationBuilder.RenameColumn(
                name: "CustomerBackgroundPhotoId",
                table: "ProductReviews",
                newName: "BackgroundPhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_CustomerBackgroundPhotoId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_BackgroundPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Backgrounds_BackgroundPhotoId",
                table: "ProductReviews",
                column: "BackgroundPhotoId",
                principalTable: "Backgrounds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Backgrounds_BackgroundPhotoId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "KnownAs",
                table: "ProductReviews",
                newName: "CustomerKnownAs");

            migrationBuilder.RenameColumn(
                name: "BackgroundPhotoId",
                table: "ProductReviews",
                newName: "CustomerBackgroundPhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_BackgroundPhotoId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_CustomerBackgroundPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Backgrounds_CustomerBackgroundPhotoId",
                table: "ProductReviews",
                column: "CustomerBackgroundPhotoId",
                principalTable: "Backgrounds",
                principalColumn: "Id");
        }
    }
}
