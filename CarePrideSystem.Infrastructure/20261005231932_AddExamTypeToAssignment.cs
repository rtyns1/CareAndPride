using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarePrideSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamTypeToAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamType",
                table: "Assignments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamType",
                table: "Assignments");
        }
    }
}
