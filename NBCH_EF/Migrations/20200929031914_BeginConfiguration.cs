using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NBCH_LIB.Models.Registrar;

namespace NBCH_EF.Migrations
{
    /// <summary>
    /// Миграция EF6 => CF Core.
    /// </summary>
    public partial class BeginConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADLoginsDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADLoginsDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ADUserDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADUserDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentGroupDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentGroupDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegionDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SellPontDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Code1C = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellPontDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfChargeDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfChargeDBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FileDescriptionDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    DocumentGroupDB_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDescriptionDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileDescriptionDBs_DocumentGroupDBs_DocumentGroupDB_ID",
                        column: x => x.DocumentGroupDB_ID,
                        principalTable: "DocumentGroupDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ADUserRegionRelations",
                columns: table => new
                {
                    RegionID = table.Column<int>(nullable: false),
                    ADUserID = table.Column<int>(nullable: false),
                    ShowToUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADUserRegionRelations", x => new { x.ADUserID, x.RegionID });
                    table.ForeignKey(
                        name: "FK_ADUserRegionRelations_ADUserDBs_ADUserID",
                        column: x => x.ADUserID,
                        principalTable: "ADUserDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ADUserRegionRelations_RegionDBs_RegionID",
                        column: x => x.RegionID,
                        principalTable: "RegionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code1C = table.Column<string>(maxLength: 15, nullable: true),
                    FirstName = table.Column<string>(maxLength: 70, nullable: true),
                    SecondName = table.Column<string>(maxLength: 70, nullable: true),
                    LastName = table.Column<string>(maxLength: 70, nullable: true),
                    FIO = table.Column<string>(maxLength: 150, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Regions_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClientDBs_RegionDBs_Regions_ID",
                        column: x => x.Regions_ID,
                        principalTable: "RegionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ADRoleDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(maxLength: 256, nullable: false),
                    FileDescriptionDB_ID = table.Column<int>(nullable: true),
                    FileDescriptionDB_ID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADRoleDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ADRoleDBs_FileDescriptionDBs_FileDescriptionDB_ID",
                        column: x => x.FileDescriptionDB_ID,
                        principalTable: "FileDescriptionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ADRoleDBs_FileDescriptionDBs_FileDescriptionDB_ID1",
                        column: x => x.FileDescriptionDB_ID1,
                        principalTable: "FileDescriptionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account1C",
                columns: table => new
                {
                    Account1CCode = table.Column<string>(maxLength: 128, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Payments = table.Column<double>(nullable: false),
                    PDNError = table.Column<bool>(nullable: false),
                    PDNManual = table.Column<bool>(nullable: false),
                    PDNAccept = table.Column<bool>(nullable: false),
                    PDNCreditHistoryAnket = table.Column<int>(nullable: false),
                    City_ID = table.Column<int>(nullable: true),
                    Client_ID = table.Column<int>(nullable: true),
                    Organization_ID = table.Column<int>(nullable: false),
                    SellPont_ID = table.Column<int>(nullable: false),
                    AdditionAgrement = table.Column<bool>(nullable: false),
                    TypeOfCharge_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account1C", x => x.Account1CCode);
                    table.ForeignKey(
                        name: "FK_Account1C_Cities_City_ID",
                        column: x => x.City_ID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account1C_ClientDBs_Client_ID",
                        column: x => x.Client_ID,
                        principalTable: "ClientDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account1C_OrganizationDBs_Organization_ID",
                        column: x => x.Organization_ID,
                        principalTable: "OrganizationDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account1C_SellPontDBs_SellPont_ID",
                        column: x => x.SellPont_ID,
                        principalTable: "SellPontDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account1C_TypeOfChargeDBs_TypeOfCharge_ID",
                        column: x => x.TypeOfCharge_ID,
                        principalTable: "TypeOfChargeDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PDFFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Client_ID = table.Column<int>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false),
                    ADUser_ID = table.Column<int>(nullable: false),
                    Region_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDFFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PDFFiles_ADUserDBs_ADUser_ID",
                        column: x => x.ADUser_ID,
                        principalTable: "ADUserDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PDFFiles_ClientDBs_Client_ID",
                        column: x => x.Client_ID,
                        principalTable: "ClientDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PDFFiles_RegionDBs_Region_ID",
                        column: x => x.Region_ID,
                        principalTable: "RegionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Client_ID = table.Column<int>(nullable: false),
                    Account1CID_Account1CCode = table.Column<string>(nullable: true),
                    SignedXML = table.Column<byte[]>(nullable: false),
                    UnSignedXML = table.Column<byte[]>(nullable: true),
                    ClientTimeZone = table.Column<int>(nullable: false),
                    ErrorCode = table.Column<string>(nullable: true),
                    ErrorText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CreditHistories_Account1C_Account1CID_Account1CCode",
                        column: x => x.Account1CID_Account1CCode,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditHistories_ClientDBs_Client_ID",
                        column: x => x.Client_ID,
                        principalTable: "ClientDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuarantorDBs",
                columns: table => new
                {
                    ClientDBID = table.Column<int>(nullable: false),
                    Account1CID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuarantorDBs", x => new { x.Account1CID, x.ClientDBID });
                    table.ForeignKey(
                        name: "FK_GuarantorDBs_Account1C_Account1CID",
                        column: x => x.Account1CID,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuarantorDBs_ClientDBs_ClientDBID",
                        column: x => x.ClientDBID,
                        principalTable: "ClientDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PDNDatas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account1C_Account1CCode = table.Column<string>(nullable: false),
                    PDNCalculateType = table.Column<int>(nullable: false),
                    CreditLimit = table.Column<double>(nullable: false),
                    AmtOutstanding = table.Column<double>(nullable: false),
                    AmtPastDue = table.Column<double>(nullable: false),
                    PSK = table.Column<double>(nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NoSrz = table.Column<bool>(nullable: false),
                    Payment = table.Column<double>(nullable: false),
                    AccountDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Error = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDNDatas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PDNDatas_Account1C_Account1C_Account1CCode",
                        column: x => x.Account1C_Account1CCode,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PDNResultDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account1C_Account1CCode = table.Column<string>(nullable: false),
                    Percent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDNResultDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PDNResultDBs_Account1C_Account1C_Account1CCode",
                        column: x => x.Account1C_Account1CCode,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostsDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_Account1CCode = table.Column<string>(nullable: true),
                    Author_ID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Message = table.Column<string>(maxLength: 512, nullable: true),
                    AuthorPermissionLevel = table.Column<int>(nullable: false),
                    ClientTimeZone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostsDBs_Account1C_Account_Account1CCode",
                        column: x => x.Account_Account1CCode,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostsDBs_ADLoginsDBs_Author_ID",
                        column: x => x.Author_ID,
                        principalTable: "ADLoginsDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrarFileDBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account1C_Account1CCode = table.Column<string>(nullable: false),
                    Client_ID = table.Column<int>(nullable: false),
                    FileDescriptionDB_ID = table.Column<int>(nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuthorName_ID = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(maxLength: 128, nullable: false),
                    File = table.Column<byte[]>(nullable: false),
                    TimeZone = table.Column<int>(nullable: false),
                    Delete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrarFileDBs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegistrarFileDBs_Account1C_Account1C_Account1CCode",
                        column: x => x.Account1C_Account1CCode,
                        principalTable: "Account1C",
                        principalColumn: "Account1CCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrarFileDBs_ADLoginsDBs_AuthorName_ID",
                        column: x => x.AuthorName_ID,
                        principalTable: "ADLoginsDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrarFileDBs_ClientDBs_Client_ID",
                        column: x => x.Client_ID,
                        principalTable: "ClientDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrarFileDBs_FileDescriptionDBs_FileDescriptionDB_ID",
                        column: x => x.FileDescriptionDB_ID,
                        principalTable: "FileDescriptionDBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account1C_City_ID",
                table: "Account1C",
                column: "City_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Account1C_Client_ID",
                table: "Account1C",
                column: "Client_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Account1C_Organization_ID",
                table: "Account1C",
                column: "Organization_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Account1C_SellPont_ID",
                table: "Account1C",
                column: "SellPont_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Account1C_TypeOfCharge_ID",
                table: "Account1C",
                column: "TypeOfCharge_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ADLoginsDBs_Login",
                table: "ADLoginsDBs",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ADRoleDBs_FileDescriptionDB_ID",
                table: "ADRoleDBs",
                column: "FileDescriptionDB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ADRoleDBs_FileDescriptionDB_ID1",
                table: "ADRoleDBs",
                column: "FileDescriptionDB_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_ADUserRegionRelations_RegionID",
                table: "ADUserRegionRelations",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDBs_Code1C",
                table: "ClientDBs",
                column: "Code1C",
                unique: true,
                filter: "[Code1C] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDBs_Regions_ID",
                table: "ClientDBs",
                column: "Regions_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditHistories_Account1CID_Account1CCode",
                table: "CreditHistories",
                column: "Account1CID_Account1CCode");

            migrationBuilder.CreateIndex(
                name: "IX_CreditHistories_Client_ID",
                table: "CreditHistories",
                column: "Client_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FileDescriptionDBs_DocumentGroupDB_ID",
                table: "FileDescriptionDBs",
                column: "DocumentGroupDB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_GuarantorDBs_ClientDBID",
                table: "GuarantorDBs",
                column: "ClientDBID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationDBs_Name",
                table: "OrganizationDBs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PDFFiles_ADUser_ID",
                table: "PDFFiles",
                column: "ADUser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PDFFiles_Client_ID",
                table: "PDFFiles",
                column: "Client_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PDFFiles_Region_ID",
                table: "PDFFiles",
                column: "Region_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PDNDatas_Account1C_Account1CCode",
                table: "PDNDatas",
                column: "Account1C_Account1CCode");

            migrationBuilder.CreateIndex(
                name: "IX_PDNResultDBs_Account1C_Account1CCode",
                table: "PDNResultDBs",
                column: "Account1C_Account1CCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostsDBs_Account_Account1CCode",
                table: "PostsDBs",
                column: "Account_Account1CCode");

            migrationBuilder.CreateIndex(
                name: "IX_PostsDBs_Author_ID",
                table: "PostsDBs",
                column: "Author_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrarFileDBs_Account1C_Account1CCode",
                table: "RegistrarFileDBs",
                column: "Account1C_Account1CCode");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrarFileDBs_AuthorName_ID",
                table: "RegistrarFileDBs",
                column: "AuthorName_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrarFileDBs_Client_ID",
                table: "RegistrarFileDBs",
                column: "Client_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrarFileDBs_FileDescriptionDB_ID",
                table: "RegistrarFileDBs",
                column: "FileDescriptionDB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SellPontDBs_Name",
                table: "SellPontDBs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfChargeDBs_Name",
                table: "TypeOfChargeDBs",
                column: "Name",
                unique: true);

			#region Ручное добавление объектов БД.

	        migrationBuilder.Sql($@"Insert Into dbo.DocumentGroupDBs ([Description]) values ('{Presets.DocumentGroup1CAccount}')");
            migrationBuilder.Sql($@"Insert Into dbo.FileDescriptionDBs ([Description], [DocumentGroupDB_ID], [SortOrder]) values ('{Presets.Photo}', (select distinct id from dbo.DocumentGroupDBs where Description = '{Presets.DocumentGroup1CAccount}'), 3 )");
            migrationBuilder.Sql($@"Insert Into dbo.TypeOfChargeDBs ([Name]) Values('{Presets.TypeOfChargeCash}')");
            
            #region TYPE dbo.Account1CList
            migrationBuilder.Sql(@"
				CREATE TYPE dbo.Account1CList
				AS TABLE (
					Code1C varchar(10)
				)"
            );
            #endregion

            #region Procedure RegistrarRoles
            migrationBuilder.Sql(@"
	            Create Procedure RegistrarRoles
				As
				Begin
					SELECT fd.[ID] FDescID,
						[Description] FDescDescription,
						rr.[Role] ReadAccess,
						null WriteAccess,
						SortOrder
					FROM [FileDescriptionDBs] fd
						Left Join [ADRoleDBs] rr on rr.FileDescriptionDB_ID = fd.ID
					Union
					SELECT fd.[ID] FDescID,
						[Description] FDescDescription,
						null,
						wr.[Role],
						SortOrder
					FROM [FileDescriptionDBs] fd
						Left Join [ADRoleDBs] wr on wr.FileDescriptionDB_ID1 = fd.ID
				End"
            );
            #endregion

            #region Procedure [dbo].[RegistrarGetFilesList]
            migrationBuilder.Sql(@"
				Create Procedure [dbo].[RegistrarGetFilesList] @accountCode varchar(15), @client1CCode varchar(15), @documentID int
				As
				Begin
					SELECT TOP(1000) fd.[ID] FDescID,
						[Description] FDescDescription,
						case
							when client.Code1C is null then null
							else fr.[ID]
						end FRegitrarID,
						case
							when client.Code1C is null then null
							else fr.[FileName]
						end[FileName],
						case
							when client.Code1C is null then null
							else fr.[UploadDate]
						end[UploadDate],
						[SortOrder],
						case
							when client.Code1C is null then null
							else logins.[Name]
						end InspectorName,
						client.Code1C Client1CCode
					FROM[FileDescriptionDBs] fd
					   Left Join[RegistrarFileDBs] fr on fd.ID = fr.FileDescriptionDB_ID and fr.[Delete] = 0 and fr.Account1C_Account1CCode = @accountCode
						Left Join[ClientDBs] client on client.ID = fr.Client_ID and client.Code1C = @client1CCode
						Left Join[ADLoginsDBs] logins on logins.ID = fr.AuthorName_ID
					Where(fd.ID = @documentID or @documentID = 0)
					Order By[SortOrder]
				End"
            );
            #endregion

            #region  Procedure [dbo].[RegistrarGetDocumentsFlag]
            migrationBuilder.Sql(@"
				CREATE Procedure [dbo].[RegistrarGetDocumentsFlag]
				@accountList dbo.Account1CList READONLY
				AS
				BEGIN

				Select Code1C As Account1CCode,
					case
						when Account1C.Client_ID is Null then Account1C2.Client_ID
						else Account1C.Client_ID
					end 'ClientID',
					case
						when RF1.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Contract',
					case
						when RF2.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Profile',
					case
						when RF3.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'PaymentSchedule',
					case
						when RF4.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'AddionalAgreement',
					case
						when RF5.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Passport',
					case
						when RF6.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'CashWarrant',
					case
						when RF7.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'NBCH',
					case
						when RF8.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'OverScans',
					case
						when RF9.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Photo',
					case
						when Account1C.Account1CCode is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Cash'
				
				From @accountList
					Left Join RegistrarFileDBs RF1 on RF1.FileDescriptionDB_ID = 1 and Code1C = RF1.Account1C_Account1CCode and RF1.[Delete] = 0
					Left Join RegistrarFileDBs RF2 on RF2.FileDescriptionDB_ID = 2 and Code1C = RF2.Account1C_Account1CCode and RF2.[Delete] = 0
					Left Join RegistrarFileDBs RF3 on RF3.FileDescriptionDB_ID = 3 and Code1C = RF3.Account1C_Account1CCode and RF3.[Delete] = 0
					Left Join RegistrarFileDBs RF4 on RF4.FileDescriptionDB_ID = 4 and Code1C = RF4.Account1C_Account1CCode and RF4.[Delete] = 0
					Left Join RegistrarFileDBs RF5 on RF5.FileDescriptionDB_ID = 5 and Code1C = RF5.Account1C_Account1CCode and RF5.[Delete] = 0
					Left Join RegistrarFileDBs RF6 on RF6.FileDescriptionDB_ID = 6 and Code1C = RF6.Account1C_Account1CCode and RF6.[Delete] = 0
					Left Join RegistrarFileDBs RF7 on RF7.FileDescriptionDB_ID = 7 and Code1C = RF7.Account1C_Account1CCode and RF7.[Delete] = 0
					Left Join RegistrarFileDBs RF8 on RF8.FileDescriptionDB_ID = 8 and Code1C = RF8.Account1C_Account1CCode and RF8.[Delete] = 0
					Left Join RegistrarFileDBs RF9 on RF9.FileDescriptionDB_ID = 9 and Code1C = RF9.Account1C_Account1CCode and RF9.[Delete] = 0
					Left Join Account1C on Account1C.Account1CCode = Code1C and Account1C.[TypeOfCharge_ID] = 1
					Left Join Account1C Account1C2 on Account1C2.Account1CCode = Code1C and Account1C2.[TypeOfCharge_ID] <> 0
				Group By Code1C,
					case
						when Account1C.Client_ID is Null then Account1C2.Client_ID
						else Account1C.Client_ID
					end,
					case
						when RF1.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF2.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF3.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF4.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF5.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF6.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF7.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF8.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF9.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when Account1C.Account1CCode is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end

					Union All

				Select Code1C As Account1CCode,
					GuarantorDBs.[ClientDBID] ClientID,
					case
						when RF1.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Contract',
					case
						when RF2.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Profile',
					case
						when RF3.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'PaymentSchedule',
					case
						when RF4.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'AddionalAgreement',
					case
						when RF5.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Passport',
					case
						when RF6.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'CashWarrant',
					case
						when RF7.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'NBCH',
					case
						when RF8.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'OverScans',
					case
						when RF9.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Photo',
					case
						when Account1C.Account1CCode is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end 'Cash'
				
				From @accountList
					Left Join RegistrarFileDBs RF1 on RF1.FileDescriptionDB_ID = 1 and Code1C = RF1.Account1C_Account1CCode and RF1.[Delete] = 0
					Left Join RegistrarFileDBs RF2 on RF2.FileDescriptionDB_ID = 2 and Code1C = RF2.Account1C_Account1CCode and RF2.[Delete] = 0
					Left Join RegistrarFileDBs RF3 on RF3.FileDescriptionDB_ID = 3 and Code1C = RF3.Account1C_Account1CCode and RF3.[Delete] = 0
					Left Join RegistrarFileDBs RF4 on RF4.FileDescriptionDB_ID = 4 and Code1C = RF4.Account1C_Account1CCode and RF4.[Delete] = 0
					Left Join RegistrarFileDBs RF5 on RF5.FileDescriptionDB_ID = 5 and Code1C = RF5.Account1C_Account1CCode and RF5.[Delete] = 0
					Left Join RegistrarFileDBs RF6 on RF6.FileDescriptionDB_ID = 6 and Code1C = RF6.Account1C_Account1CCode and RF6.[Delete] = 0
					Left Join RegistrarFileDBs RF7 on RF7.FileDescriptionDB_ID = 7 and Code1C = RF7.Account1C_Account1CCode and RF7.[Delete] = 0
					Left Join RegistrarFileDBs RF8 on RF8.FileDescriptionDB_ID = 8 and Code1C = RF8.Account1C_Account1CCode and RF8.[Delete] = 0
					Left Join RegistrarFileDBs RF9 on RF9.FileDescriptionDB_ID = 9 and Code1C = RF9.Account1C_Account1CCode and RF9.[Delete] = 0
					Left Join Account1C on Account1C.Account1CCode = Code1C and Account1C.[TypeOfCharge_ID] = 1
					Inner Join GuarantorDBs on GuarantorDBs.[Account1CID] = Code1C
				Group By Code1C,
					GuarantorDBs.[ClientDBID],
					case
						when RF1.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF2.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF3.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF4.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF5.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF6.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF7.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF8.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when RF9.ID is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end,
					case
						when Account1C.Account1CCode is not Null then CONVERT(bit, 1)
						else CONVERT(bit, 0)
					end

				END"
            );
            #endregion

            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder) {

			#region Ручное удаление объектов БД.
	        migrationBuilder.Sql("Drop TYPE dbo.Account1CList");
	        migrationBuilder.Sql("DROP PROCEDURE [dbo].[RegistrarRoles]");
	        migrationBuilder.Sql("DROP PROCEDURE [dbo].[RegistrarGetFilesList]");
	        migrationBuilder.Sql("DROP PROCEDURE [dbo].[RegistrarGetDocumentsFlag]");
            #endregion

            migrationBuilder.DropTable(
                name: "ADRoleDBs");

            migrationBuilder.DropTable(
                name: "ADUserRegionRelations");

            migrationBuilder.DropTable(
                name: "CreditHistories");

            migrationBuilder.DropTable(
                name: "GuarantorDBs");

            migrationBuilder.DropTable(
                name: "PDFFiles");

            migrationBuilder.DropTable(
                name: "PDNDatas");

            migrationBuilder.DropTable(
                name: "PDNResultDBs");

            migrationBuilder.DropTable(
                name: "PostsDBs");

            migrationBuilder.DropTable(
                name: "RegistrarFileDBs");

            migrationBuilder.DropTable(
                name: "ADUserDBs");

            migrationBuilder.DropTable(
                name: "Account1C");

            migrationBuilder.DropTable(
                name: "ADLoginsDBs");

            migrationBuilder.DropTable(
                name: "FileDescriptionDBs");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ClientDBs");

            migrationBuilder.DropTable(
                name: "OrganizationDBs");

            migrationBuilder.DropTable(
                name: "SellPontDBs");

            migrationBuilder.DropTable(
                name: "TypeOfChargeDBs");

            migrationBuilder.DropTable(
                name: "DocumentGroupDBs");

            migrationBuilder.DropTable(
                name: "RegionDBs");






        }
    }
}
