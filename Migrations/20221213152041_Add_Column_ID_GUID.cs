using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JavaExamFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIDGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DangKyThi",
                table: "DangKyThi");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "DangKyThi",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DangKyThi",
                table: "DangKyThi",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DangKyThi",
                table: "DangKyThi");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "DangKyThi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DangKyThi",
                table: "DangKyThi",
                column: "StudentID");
        }
    }
}
