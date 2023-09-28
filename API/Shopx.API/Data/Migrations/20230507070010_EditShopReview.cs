using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class EditShopReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopReviews_AspNetUsers_SellerId",
                table: "ShopReviews");

            migrationBuilder.CreateIndex(
                name: "IX_ShopReviews_CustomerId",
                table: "ShopReviews",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopReviews_AspNetUsers_CustomerId",
                table: "ShopReviews",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopReviews_AspNetUsers_SellerId",
                table: "ShopReviews",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopReviews_AspNetUsers_CustomerId",
                table: "ShopReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopReviews_AspNetUsers_SellerId",
                table: "ShopReviews");

            migrationBuilder.DropIndex(
                name: "IX_ShopReviews_CustomerId",
                table: "ShopReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopReviews_AspNetUsers_SellerId",
                table: "ShopReviews",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
