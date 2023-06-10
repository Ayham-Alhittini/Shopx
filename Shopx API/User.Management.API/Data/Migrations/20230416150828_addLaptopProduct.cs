using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class addLaptopProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaptopId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenSize = table.Column<double>(type: "float", nullable: false),
                    Ram = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptops", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_LaptopId",
                table: "Products",
                column: "LaptopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Laptops_LaptopId",
                table: "Products",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Laptops_LaptopId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Laptops");

            migrationBuilder.DropIndex(
                name: "IX_Products_LaptopId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LaptopId",
                table: "Products");
        }
    }
}
