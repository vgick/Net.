using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NBCH_EF.Migrations
{
    public partial class MigrateToEFCoreAddQuerysStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsForCheckQuery",
                columns: table => new
                {
                    Account1CCode = table.Column<string>(nullable: true),
                    ClientID = table.Column<int>(nullable: false),
                    NBCH = table.Column<bool>(nullable: false),
                    Passport = table.Column<bool>(nullable: false),
                    Profile = table.Column<bool>(nullable: false),
                    Contract = table.Column<bool>(nullable: false),
                    PaymentSchedule = table.Column<bool>(nullable: false),
                    AddionalAgreement = table.Column<bool>(nullable: false),
                    CashWarrant = table.Column<bool>(nullable: false),
                    OverScans = table.Column<bool>(nullable: false),
                    Photo = table.Column<bool>(nullable: false),
                    Cash = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FileDescriptionQuery",
                columns: table => new
                {
                    FDescID = table.Column<int>(nullable: false),
                    FDescDescription = table.Column<string>(nullable: true),
                    FRegitrarID = table.Column<int>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UploadDate = table.Column<DateTime>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    InspectorName = table.Column<string>(nullable: true),
                    Client1CCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RoleListQuery",
                columns: table => new
                {
                    FDescID = table.Column<int>(nullable: false),
                    FDescDescription = table.Column<string>(nullable: true),
                    ReadAccess = table.Column<string>(nullable: true),
                    WriteAccess = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsForCheckQuery");

            migrationBuilder.DropTable(
                name: "FileDescriptionQuery");

            migrationBuilder.DropTable(
                name: "RoleListQuery");
        }
    }
}
