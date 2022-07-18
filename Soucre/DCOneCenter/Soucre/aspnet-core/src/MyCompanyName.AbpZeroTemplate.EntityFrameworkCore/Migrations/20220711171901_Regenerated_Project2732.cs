using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class Regenerated_Project2732 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TokenShortname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTokenSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectSummary = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    WebsiteURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Whitepaper_URL_FAQ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reddit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telegram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YourName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YourEmailaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPormoted = table.Column<bool>(type: "bit", nullable: false),
                    CountSee = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectStatuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectIndustrieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectIndustries_ProjectIndustrieId",
                        column: x => x.ProjectIndustrieId,
                        principalTable: "ProjectIndustries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectStatus_ProjectStatuId",
                        column: x => x.ProjectStatuId,
                        principalTable: "ProjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectIndustrieId",
                table: "Projects",
                column: "ProjectIndustrieId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStatuId",
                table: "Projects",
                column: "ProjectStatuId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TenantId",
                table: "Projects",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
