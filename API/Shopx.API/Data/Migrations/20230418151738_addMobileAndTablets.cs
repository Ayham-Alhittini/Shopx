using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class addMobileAndTablets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MobilesAndTablets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenSize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilesAndTablets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MobileId",
                table: "Products",
                column: "MobileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MobilesAndTablets_MobileId",
                table: "Products",
                column: "MobileId",
                principalTable: "MobilesAndTablets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MobilesAndTablets_MobileId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "MobilesAndTablets");

            migrationBuilder.DropIndex(
                name: "IX_Products_MobileId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MobileId",
                table: "Products");
        }
    }
}
