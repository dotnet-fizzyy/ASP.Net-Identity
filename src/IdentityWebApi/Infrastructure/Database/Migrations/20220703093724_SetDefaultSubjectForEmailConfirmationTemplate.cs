using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityWebApi.Infrastructure.Database.Migrations
{
    public partial class SetDefaultSubjectForEmailConfirmationTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("f8fd1c61-584c-4c37-8be9-54d39dd1c92c"),
                column: "Subject",
                value: "Account email confirmation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("f8fd1c61-584c-4c37-8be9-54d39dd1c92c"),
                column: "Subject",
                value: null);
        }
    }
}
