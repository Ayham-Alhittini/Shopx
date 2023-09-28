using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class addMointer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonitorProductId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MonitorProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenSize = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorProduct", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MonitorProductId",
                table: "Products",
                column: "MonitorProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MonitorProduct_MonitorProductId",
                table: "Products",
                column: "MonitorProductId",
                principalTable: "MonitorProduct",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MonitorProduct_MonitorProductId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "MonitorProduct");

            migrationBuilder.DropIndex(
                name: "IX_Products_MonitorProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MonitorProductId",
                table: "Products");
        }
    }
}
