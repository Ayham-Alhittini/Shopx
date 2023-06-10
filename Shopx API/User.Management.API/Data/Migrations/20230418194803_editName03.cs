using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class editName03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Laptops_LaptopId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laptops",
                table: "Laptops");

            migrationBuilder.RenameTable(
                name: "Laptops",
                newName: "Laptops_And_Computers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laptops_And_Computers",
                table: "Laptops_And_Computers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Laptops_And_Computers_LaptopId",
                table: "Products",
                column: "LaptopId",
                principalTable: "Laptops_And_Computers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Laptops_And_Computers_LaptopId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laptops_And_Computers",
                table: "Laptops_And_Computers");

            migrationBuilder.RenameTable(
                name: "Laptops_And_Computers",
                newName: "Laptops");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laptops",
                table: "Laptops",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Laptops_LaptopId",
                table: "Products",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id");
        }
    }
}
