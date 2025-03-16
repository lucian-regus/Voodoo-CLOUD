using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScrapingLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapingLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlacklistedIpAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: true),
                    ScrapingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedIpAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlacklistedIpAddresses_ScrapingLogs_ScrapingLogId",
                        column: x => x.ScrapingLogId,
                        principalTable: "ScrapingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MalwareSignatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ScrapingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalwareSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MalwareSignatures_ScrapingLogs_ScrapingLogId",
                        column: x => x.ScrapingLogId,
                        principalTable: "ScrapingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YaraRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScrapingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YaraRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YaraRules_ScrapingLogs_ScrapingLogId",
                        column: x => x.ScrapingLogId,
                        principalTable: "ScrapingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedIpAddresses_DeletedAt",
                table: "BlacklistedIpAddresses",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedIpAddresses_ScrapingLogId",
                table: "BlacklistedIpAddresses",
                column: "ScrapingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_MalwareSignatures_DeletedAt",
                table: "MalwareSignatures",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MalwareSignatures_ScrapingLogId",
                table: "MalwareSignatures",
                column: "ScrapingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapingLogs_DeletedAt",
                table: "ScrapingLogs",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_YaraRules_DeletedAt",
                table: "YaraRules",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_YaraRules_ScrapingLogId",
                table: "YaraRules",
                column: "ScrapingLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedIpAddresses");

            migrationBuilder.DropTable(
                name: "MalwareSignatures");

            migrationBuilder.DropTable(
                name: "YaraRules");

            migrationBuilder.DropTable(
                name: "ScrapingLogs");
        }
    }
}
