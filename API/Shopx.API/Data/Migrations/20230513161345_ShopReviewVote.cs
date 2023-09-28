using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class ShopReviewVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopReviews",
                table: "ShopReviews");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "ShopReviews",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "ShopReviews",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShopReviews",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopReviews",
                table: "ShopReviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShopReviews_SellerId",
                table: "ShopReviews",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopReviews",
                table: "ShopReviews");

            migrationBuilder.DropIndex(
                name: "IX_ShopReviews_SellerId",
                table: "ShopReviews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShopReviews");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "ShopReviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "ShopReviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopReviews",
                table: "ShopReviews",
                columns: new[] { "SellerId", "CustomerId" });
        }
    }
}
