using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddVirtualServiceToApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_ApplicationId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ApplicationId",
                table: "Services",
                column: "ApplicationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_ApplicationId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ApplicationId",
                table: "Services",
                column: "ApplicationId");
        }
    }
}
