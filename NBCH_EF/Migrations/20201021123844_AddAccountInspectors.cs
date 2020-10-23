using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NBCH_EF.Migrations
{
    public partial class AddAccountInspectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountInspectingDB",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationDate = table.Column<DateTime>(nullable: false),
                    Account1CID = table.Column<string>(nullable: false),
                    ADLoginsDBID = table.Column<int>(nullable: false),
                    UserPermission = table.Column<int>(nullable: false),
                    TimeZone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInspectingDB", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountInspectingDB_ADLoginsDBs_ADLoginsDBID",
                        column: x => x.ADLoginsDBID,
                        principalTable: "ADLoginsDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInspectingDB_Account1C_Account1CID",
                        column: x => x.Account1CID,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountInspectingDB_ADLoginsDBID",
                table: "AccountInspectingDB",
                column: "ADLoginsDBID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInspectingDB_Account1CID",
                table: "AccountInspectingDB",
                column: "Account1CID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountInspectingDB");
        }
    }
}
