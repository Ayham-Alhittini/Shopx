using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class deleteBackgroundFromProductReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Backgrounds_BackgroundPhotoId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_BackgroundPhotoId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "BackgroundPhotoId",
                table: "ProductReviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackgroundPhotoId",
                table: "ProductReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_BackgroundPhotoId",
                table: "ProductReviews",
                column: "BackgroundPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Backgrounds_BackgroundPhotoId",
                table: "ProductReviews",
                column: "BackgroundPhotoId",
                principalTable: "Backgrounds",
                principalColumn: "Id");
        }
    }
}
