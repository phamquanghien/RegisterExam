using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JavaExamFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDangKyThiSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "DangKyThi",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "DangKyThi");
        }
    }
}
