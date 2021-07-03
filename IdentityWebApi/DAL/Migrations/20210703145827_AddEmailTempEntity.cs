using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityWebApi.DAL.Migrations
{
    public partial class AddEmailTempEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplate",
                columns: new[] { "Id", "CreationDate", "Layout", "Name" },
                values: new object[] { new Guid("f8fd1c61-584c-4c37-8be9-54d39dd1c92c"), new DateTime(2021, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "<div style=\"background-color: black; padding: 10px; box-sizing: border-box\">\n    <h1 style=\"color: white; font-size: 26px\">Email confirmation</h1>\n</div>\n<div style=\"background-color: white; color: black; padding: 10px; box-sizing: border-box\">\n    <span>Confirm your email registration via this <a href={{link}}>link</a>.</span>\n</div>", "EmailConfirmation" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_Name",
                table: "EmailTemplate",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplate");
        }
    }
}
