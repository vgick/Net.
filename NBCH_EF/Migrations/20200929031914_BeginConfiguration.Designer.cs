﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NBCH_EF.Migrations
{
    [DbContext(typeof(NBCH_EF.MKKContext))]
    [Migration("20200929031914_BeginConfiguration")]
    partial class BeginConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NBCH_DB.Tables.ADLoginsDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("ID");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("ADLoginsDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ADRoleDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ReadADRolesID")
                        .HasColumnName("FileDescriptionDB_ID")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("WriteADRolesID")
                        .HasColumnName("FileDescriptionDB_ID1")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReadADRolesID");

                    b.HasIndex("WriteADRolesID");

                    b.ToTable("ADRoleDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ADUserDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ADName")
                        .IsRequired()
                        .HasColumnName("ADName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("ADUserDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ADUserRegionRelation", b =>
                {
                    b.Property<int>("ADUserID")
                        .HasColumnName("ADUserID")
                        .HasColumnType("int");

                    b.Property<int>("RegionID")
                        .HasColumnName("RegionID")
                        .HasColumnType("int");

                    b.Property<bool>("ShowToUser")
                        .HasColumnType("bit");

                    b.HasKey("ADUserID", "RegionID");

                    b.HasIndex("RegionID");

                    b.ToTable("ADUserRegionRelations");
                });

            modelBuilder.Entity("NBCH_DB.Tables.Account1C", b =>
                {
                    b.Property<string>("Account1CCode")
                        .HasColumnName("Account1CCode")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<bool>("AdditionAgrement")
                        .HasColumnType("bit");

                    b.Property<int?>("CityID")
                        .HasColumnName("City_ID")
                        .HasColumnType("int");

                    b.Property<int?>("ClientID")
                        .HasColumnName("Client_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("OrganizationID")
                        .HasColumnName("Organization_ID")
                        .HasColumnType("int");

                    b.Property<bool>("PDNAccept")
                        .HasColumnName("PDNAccept")
                        .HasColumnType("bit");

                    b.Property<int>("PDNCreditHistoryAnket")
                        .HasColumnName("PDNCreditHistoryAnket")
                        .HasColumnType("int");

                    b.Property<bool>("PDNError")
                        .HasColumnName("PDNError")
                        .HasColumnType("bit");

                    b.Property<bool>("PDNManual")
                        .HasColumnName("PDNManual")
                        .HasColumnType("bit");

                    b.Property<double>("Payments")
                        .HasColumnType("float");

                    b.Property<int>("SellPontID")
                        .HasColumnName("SellPont_ID")
                        .HasColumnType("int");

                    b.Property<int?>("TypeOfChargeID")
                        .HasColumnName("TypeOfCharge_ID")
                        .HasColumnType("int");

                    b.HasKey("Account1CCode");

                    b.HasIndex("CityID");

                    b.HasIndex("ClientID");

                    b.HasIndex("OrganizationID");

                    b.HasIndex("SellPontID");

                    b.HasIndex("TypeOfChargeID");

                    b.ToTable("Account1C");
                });

            modelBuilder.Entity("NBCH_DB.Tables.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ClientDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Code1C")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("FIO")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<int?>("RegionsId")
                        .HasColumnName("Regions_ID")
                        .HasColumnType("int");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.HasKey("ID");

                    b.HasIndex("Code1C")
                        .IsUnique()
                        .HasFilter("[Code1C] IS NOT NULL");

                    b.HasIndex("RegionsId");

                    b.ToTable("ClientDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.CreditHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account1CidAccount1CCode")
                        .HasColumnName("Account1CID_Account1CCode")
                        .HasColumnType("nvarchar(128)");

                    b.Property<int?>("ClientID")
                        .IsRequired()
                        .HasColumnName("Client_ID")
                        .HasColumnType("int");

                    b.Property<int>("ClientTimeZone")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("ErrorCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("SignedXML")
                        .IsRequired()
                        .HasColumnName("SignedXML")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("UnSignedXML")
                        .HasColumnName("UnSignedXML")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ID");

                    b.HasIndex("Account1CidAccount1CCode");

                    b.HasIndex("ClientID");

                    b.ToTable("CreditHistories");
                });

            modelBuilder.Entity("NBCH_DB.Tables.DocumentGroupDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.ToTable("DocumentGroupDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.FileDescriptionDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("DocumentGroupDBID")
                        .HasColumnName("DocumentGroupDB_ID")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DocumentGroupDBID");

                    b.ToTable("FileDescriptionDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.GuarantorDB", b =>
                {
                    b.Property<string>("Account1CID")
                        .HasColumnName("Account1CID")
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("ClientDBID")
                        .HasColumnName("ClientDBID")
                        .HasColumnType("int");

                    b.HasKey("Account1CID", "ClientDBID");

                    b.HasIndex("ClientDBID");

                    b.ToTable("GuarantorDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.OrganizationDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("OrganizationDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDFFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AduserID")
                        .HasColumnName("ADUser_ID")
                        .HasColumnType("int");

                    b.Property<int>("ClientID")
                        .HasColumnName("Client_ID")
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("RegionID")
                        .HasColumnName("Region_ID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AduserID");

                    b.HasIndex("ClientID");

                    b.HasIndex("RegionID");

                    b.ToTable("PDFFiles");
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDNData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account1CAccount1CCode")
                        .IsRequired()
                        .HasColumnName("Account1C_Account1CCode")
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("AccountDate")
                        .HasColumnType("datetime");

                    b.Property<double>("AmtOutstanding")
                        .HasColumnType("float");

                    b.Property<double>("AmtPastDue")
                        .HasColumnType("float");

                    b.Property<double>("CreditLimit")
                        .HasColumnType("float");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<bool>("NoSrz")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("OpenDate")
                        .HasColumnType("datetime");

                    b.Property<int>("PDNCalculateType")
                        .HasColumnName("PDNCalculateType")
                        .HasColumnType("int");

                    b.Property<double>("PSK")
                        .HasColumnName("PSK")
                        .HasColumnType("float");

                    b.Property<double>("Payment")
                        .HasColumnType("float");

                    b.Property<DateTime?>("PaymentDueDate")
                        .HasColumnType("datetime");

                    b.HasKey("ID");

                    b.HasIndex("Account1CAccount1CCode");

                    b.ToTable("PDNDatas");
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDNResultDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account1CID")
                        .IsRequired()
                        .HasColumnName("Account1C_Account1CCode")
                        .HasColumnType("nvarchar(128)");

                    b.Property<double>("Percent")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("Account1CID")
                        .IsUnique();

                    b.ToTable("PDNResultDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.PostsDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountAccount1CCode")
                        .HasColumnName("Account_Account1CCode")
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("AuthorID")
                        .HasColumnName("Author_ID")
                        .HasColumnType("int");

                    b.Property<int>("AuthorPermissionLevel")
                        .HasColumnType("int");

                    b.Property<int>("ClientTimeZone")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.HasKey("ID");

                    b.HasIndex("AccountAccount1CCode");

                    b.HasIndex("AuthorID");

                    b.ToTable("PostsDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.RegionDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("RegionDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.RegistrarFileDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account1CAccount1CCode")
                        .IsRequired()
                        .HasColumnName("Account1C_Account1CCode")
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("AuthorNameID")
                        .HasColumnName("AuthorName_ID")
                        .HasColumnType("int");

                    b.Property<int>("ClientID")
                        .HasColumnName("Client_ID")
                        .HasColumnType("int");

                    b.Property<bool>("Delete")
                        .HasColumnType("bit");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("FileDescriptionDbID")
                        .HasColumnName("FileDescriptionDB_ID")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("TimeZone")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime");

                    b.HasKey("ID");

                    b.HasIndex("Account1CAccount1CCode");

                    b.HasIndex("AuthorNameID");

                    b.HasIndex("ClientID");

                    b.HasIndex("FileDescriptionDbID");

                    b.ToTable("RegistrarFileDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.SellPontDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code1C")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SellPontDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.TypeOfChargeDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TypeOfChargeDBs");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ADRoleDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.FileDescriptionDB", "ReadADRoles")
                        .WithMany("ReadADRoles")
                        .HasForeignKey("ReadADRolesID");

                    b.HasOne("NBCH_DB.Tables.FileDescriptionDB", "WriteADRoles")
                        .WithMany("WriteADRoles")
                        .HasForeignKey("WriteADRolesID");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ADUserRegionRelation", b =>
                {
                    b.HasOne("NBCH_DB.Tables.ADUserDB", "ADUser")
                        .WithMany("ADUserRegionRelations")
                        .HasForeignKey("ADUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.RegionDB", "Region")
                        .WithMany("ADUserRegionRelations")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.Account1C", b =>
                {
                    b.HasOne("NBCH_DB.Tables.City", "City")
                        .WithMany("Account1C")
                        .HasForeignKey("CityID");

                    b.HasOne("NBCH_DB.Tables.ClientDB", "Client")
                        .WithMany("Account1C")
                        .HasForeignKey("ClientID");

                    b.HasOne("NBCH_DB.Tables.OrganizationDB", "Organization")
                        .WithMany("Account1C")
                        .HasForeignKey("OrganizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.SellPontDB", "SellPont")
                        .WithMany("Account1C")
                        .HasForeignKey("SellPontID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.TypeOfChargeDB", "TypeOfCharge")
                        .WithMany("Account1C")
                        .HasForeignKey("TypeOfChargeID");
                });

            modelBuilder.Entity("NBCH_DB.Tables.ClientDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.RegionDB", "Regions")
                        .WithMany()
                        .HasForeignKey("RegionsId");
                });

            modelBuilder.Entity("NBCH_DB.Tables.CreditHistory", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account1CID")
                        .WithMany()
                        .HasForeignKey("Account1CidAccount1CCode");

                    b.HasOne("NBCH_DB.Tables.ClientDB", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.FileDescriptionDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.DocumentGroupDB", "DocumentGroupDB")
                        .WithMany("FileDescriptionDB")
                        .HasForeignKey("DocumentGroupDBID");
                });

            modelBuilder.Entity("NBCH_DB.Tables.GuarantorDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account")
                        .WithMany("GuarantorDBs")
                        .HasForeignKey("Account1CID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.ClientDB", "Client")
                        .WithMany("GuarantorDBs")
                        .HasForeignKey("ClientDBID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDFFile", b =>
                {
                    b.HasOne("NBCH_DB.Tables.ADUserDB", "ADUser")
                        .WithMany()
                        .HasForeignKey("AduserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.ClientDB", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.RegionDB", "Region")
                        .WithMany()
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDNData", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account1C")
                        .WithMany("PDNData")
                        .HasForeignKey("Account1CAccount1CCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.PDNResultDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account1C")
                        .WithMany()
                        .HasForeignKey("Account1CID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.PostsDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account")
                        .WithMany()
                        .HasForeignKey("AccountAccount1CCode");

                    b.HasOne("NBCH_DB.Tables.ADLoginsDB", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NBCH_DB.Tables.RegistrarFileDB", b =>
                {
                    b.HasOne("NBCH_DB.Tables.Account1C", "Account1C")
                        .WithMany()
                        .HasForeignKey("Account1CAccount1CCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.ADLoginsDB", "AuthorName")
                        .WithMany()
                        .HasForeignKey("AuthorNameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.ClientDB", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBCH_DB.Tables.FileDescriptionDB", "FileDescriptionDB")
                        .WithMany()
                        .HasForeignKey("FileDescriptionDbID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}