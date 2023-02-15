using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemoveAdminFromApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Admins_AdminId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AdminId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Applications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AdminId",
                table: "Applications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AdminId",
                table: "Applications",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Admins_AdminId",
                table: "Applications",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
