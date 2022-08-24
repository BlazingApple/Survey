using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingAppleConsumer.Server.Migrations
{
    public partial class ChangePropertyNamesPartTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyItems_QuestionId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItemOptions_SurveyItems_QuestionId",
                table: "SurveyItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItems_Surveys_SurveyId",
                table: "SurveyItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyItems",
                table: "SurveyItems");

            migrationBuilder.DropColumn(
                name: "ItemValue",
                table: "SurveyItems");

            migrationBuilder.RenameTable(
                name: "SurveyItems",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyItems_SurveyId",
                table: "Questions",
                newName: "IX_Questions_SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_Questions_QuestionId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItemOptions_Questions_QuestionId",
                table: "SurveyItemOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "SurveyItems");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_SurveyId",
                table: "SurveyItems",
                newName: "IX_SurveyItems_SurveyId");

            migrationBuilder.AddColumn<string>(
                name: "ItemValue",
                table: "SurveyItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyItems",
                table: "SurveyItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyItems_QuestionId",
                table: "SurveyAnswers",
                column: "QuestionId",
                principalTable: "SurveyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyItemOptions_SurveyItems_QuestionId",
                table: "SurveyItemOptions",
                column: "QuestionId",
                principalTable: "SurveyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyItems_Surveys_SurveyId",
                table: "SurveyItems",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
