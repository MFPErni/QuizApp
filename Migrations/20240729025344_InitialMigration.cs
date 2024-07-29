using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntroBE.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminList",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminList", x => x.AdminID);
                });

            migrationBuilder.CreateTable(
                name: "GuestList",
                columns: table => new
                {
                    GuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestList", x => x.GuestID);
                });

            migrationBuilder.CreateTable(
                name: "QuizList",
                columns: table => new
                {
                    QuizID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizList", x => x.QuizID);
                    table.ForeignKey(
                        name: "FK_QuizList_AdminList_AdminID",
                        column: x => x.AdminID,
                        principalTable: "AdminList",
                        principalColumn: "AdminID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestQuizScoreList",
                columns: table => new
                {
                    GuestQuizScoreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestID = table.Column<int>(type: "int", nullable: false),
                    QuizID = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestQuizScoreList", x => x.GuestQuizScoreID);
                    table.ForeignKey(
                        name: "FK_GuestQuizScoreList_GuestList_GuestID",
                        column: x => x.GuestID,
                        principalTable: "GuestList",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestQuizScoreList_QuizList_QuizID",
                        column: x => x.QuizID,
                        principalTable: "QuizList",
                        principalColumn: "QuizID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionList",
                columns: table => new
                {
                    QuestionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizID = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionList", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_QuestionList_QuizList_QuizID",
                        column: x => x.QuizID,
                        principalTable: "QuizList",
                        principalColumn: "QuizID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerList",
                columns: table => new
                {
                    AnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionID = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerList", x => x.AnswerID);
                    table.ForeignKey(
                        name: "FK_AnswerList_QuestionList_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "QuestionList",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerList_QuestionID",
                table: "AnswerList",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_GuestQuizScoreList_GuestID",
                table: "GuestQuizScoreList",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_GuestQuizScoreList_QuizID",
                table: "GuestQuizScoreList",
                column: "QuizID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionList_QuizID",
                table: "QuestionList",
                column: "QuizID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizList_AdminID",
                table: "QuizList",
                column: "AdminID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerList");

            migrationBuilder.DropTable(
                name: "GuestQuizScoreList");

            migrationBuilder.DropTable(
                name: "QuestionList");

            migrationBuilder.DropTable(
                name: "GuestList");

            migrationBuilder.DropTable(
                name: "QuizList");

            migrationBuilder.DropTable(
                name: "AdminList");
        }
    }
}
