using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JavaExamFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStudentSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Student");
        }
    }
}
