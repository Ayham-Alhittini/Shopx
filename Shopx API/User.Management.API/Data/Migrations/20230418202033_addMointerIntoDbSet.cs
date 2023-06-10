using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class addMointerIntoDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MonitorProduct_MonitorProductId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorProduct",
                table: "MonitorProduct");

            migrationBuilder.RenameTable(
                name: "MonitorProduct",
                newName: "MonitorProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorProducts",
                table: "MonitorProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MonitorProducts_MonitorProductId",
                table: "Products",
                column: "MonitorProductId",
                principalTable: "MonitorProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MonitorProducts_MonitorProductId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorProducts",
                table: "MonitorProducts");

            migrationBuilder.RenameTable(
                name: "MonitorProducts",
                newName: "MonitorProduct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorProduct",
                table: "MonitorProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MonitorProduct_MonitorProductId",
                table: "Products",
                column: "MonitorProductId",
                principalTable: "MonitorProduct",
                principalColumn: "Id");
        }
    }
}
