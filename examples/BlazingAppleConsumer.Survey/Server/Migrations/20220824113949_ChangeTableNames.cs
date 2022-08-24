using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingAppleConsumer.Server.Migrations
{
    public partial class ChangeTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_Questions_QuestionId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItemOptions_Questions_QuestionId",
                table: "SurveyItemOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyItemOptions",
                table: "SurveyItemOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyAnswers",
                table: "SurveyAnswers");

            migrationBuilder.RenameTable(
                name: "SurveyItemOptions",
                newName: "QuestionOptions");

            migrationBuilder.RenameTable(
                name: "SurveyAnswers",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyItemOptions_QuestionId",
                table: "QuestionOptions",
                newName: "IX_QuestionOptions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswers_QuestionId",
                table: "Answers",
                newName: "IX_Answers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "QuestionOptions",
                newName: "SurveyItemOptions");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "SurveyAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "SurveyItemOptions",
                newName: "IX_SurveyItemOptions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionId",
                table: "SurveyAnswers",
                newName: "IX_SurveyAnswers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyItemOptions",
                table: "SurveyItemOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyAnswers",
                table: "SurveyAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_Questions_QuestionId",
                table: "SurveyAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyItemOptions_Questions_QuestionId",
                table: "SurveyItemOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
