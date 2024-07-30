using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntroBE.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "QuizList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "QuizList");
        }
    }
}
