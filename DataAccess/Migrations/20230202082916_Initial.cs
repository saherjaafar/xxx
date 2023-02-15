using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadIcon = table.Column<bool>(type: "bit", nullable: false),
                    WriteIcon = table.Column<bool>(type: "bit", nullable: false),
                    ReadUsers = table.Column<bool>(type: "bit", nullable: false),
                    ManageUsers = table.Column<bool>(type: "bit", nullable: false),
                    ReadServiceCategory = table.Column<bool>(type: "bit", nullable: false),
                    ManageServiceCategory = table.Column<bool>(type: "bit", nullable: false),
                    ReadCountry = table.Column<bool>(type: "bit", nullable: false),
                    ManageCountry = table.Column<bool>(type: "bit", nullable: false),
                    ReadIcons = table.Column<bool>(type: "bit", nullable: false),
                    ManageIcons = table.Column<bool>(type: "bit", nullable: false),
                    ReadApplication = table.Column<bool>(type: "bit", nullable: false),
                    ManageApplication = table.Column<bool>(type: "bit", nullable: false),
                    ReadService = table.Column<bool>(type: "bit", nullable: false),
                    ManageService = table.Column<bool>(type: "bit", nullable: false),
                    ReadSponsor = table.Column<bool>(type: "bit", nullable: false),
                    ManageSponsor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Icons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Svg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    AdminRoleId = table.Column<long>(type: "bigint", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Admins_AdminRoles_AdminRoleId",
                        column: x => x.AdminRoleId,
                        principalTable: "AdminRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Icons_IconId",
                        column: x => x.IconId,
                        principalTable: "Icons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IconId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_Icons_IconId",
                        column: x => x.IconId,
                        principalTable: "Icons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminVerification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailVerificationToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    ResetPasswordToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResetPasswordTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminVerification_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryFilters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    EnumKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryFilters_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryDistricts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryDistricts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryDistricts",
                columns: table => new
                {
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CountryDistrictId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDistricts", x => new { x.CountryDistrictId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryDistricts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryDistricts_CountryDistricts_CountryDistrictId",
                        column: x => x.CountryDistrictId,
                        principalTable: "CountryDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistrictStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryDistrictId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistrictStates_CountryDistricts_CountryDistrictId",
                        column: x => x.CountryDistrictId,
                        principalTable: "CountryDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminRoleId",
                table: "Admins",
                column: "AdminRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminVerification_AdminId",
                table: "AdminVerification",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IconId",
                table: "Categories",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDistricts_CategoryId",
                table: "CategoryDistricts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFilters_CategoryId",
                table: "CategoryFilters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IconId",
                table: "Countries",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryDistricts_CountryId",
                table: "CountryDistricts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictStates_CountryDistrictId",
                table: "DistrictStates",
                column: "CountryDistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminVerification");

            migrationBuilder.DropTable(
                name: "CategoryDistricts");

            migrationBuilder.DropTable(
                name: "CategoryFilters");

            migrationBuilder.DropTable(
                name: "DistrictStates");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CountryDistricts");

            migrationBuilder.DropTable(
                name: "AdminRoles");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Icons");
        }
    }
}
