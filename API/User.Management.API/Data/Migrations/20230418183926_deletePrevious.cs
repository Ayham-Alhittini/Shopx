using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class deletePrevious : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MobileAccessoriesAndSpareParts_MobileAccessoriesAndSparePartsId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "MobileAccessoriesAndSpareParts");

            migrationBuilder.DropIndex(
                name: "IX_Products_MobileAccessoriesAndSparePartsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MobileAccessoriesAndSparePartsId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileAccessoriesAndSparePartsId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MobileAccessoriesAndSpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAccessoriesAndSpareParts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MobileAccessoriesAndSparePartsId",
                table: "Products",
                column: "MobileAccessoriesAndSparePartsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MobileAccessoriesAndSpareParts_MobileAccessoriesAndSparePartsId",
                table: "Products",
                column: "MobileAccessoriesAndSparePartsId",
                principalTable: "MobileAccessoriesAndSpareParts",
                principalColumn: "Id");
        }
    }
}
