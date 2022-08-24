using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingAppleConsumer.Server.Migrations
{
    public partial class ChangePropertyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyItems_SurveyItemId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItemOptions_SurveyItems_SurveyItemId",
                table: "SurveyItemOptions");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "SurveyItems",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "SurveyItemId",
                table: "SurveyItemOptions",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyItemOptions_SurveyItemId",
                table: "SurveyItemOptions",
                newName: "IX_SurveyItemOptions_QuestionId");

            migrationBuilder.RenameColumn(
                name: "SurveyItemId",
                table: "SurveyAnswers",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswers_SurveyItemId",
                table: "SurveyAnswers",
                newName: "IX_SurveyAnswers_QuestionId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyItems_QuestionId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyItemOptions_SurveyItems_QuestionId",
                table: "SurveyItemOptions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SurveyItems",
                newName: "ItemType");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "SurveyItemOptions",
                newName: "SurveyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyItemOptions_QuestionId",
                table: "SurveyItemOptions",
                newName: "IX_SurveyItemOptions_SurveyItemId");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "SurveyAnswers",
                newName: "SurveyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswers_QuestionId",
                table: "SurveyAnswers",
                newName: "IX_SurveyAnswers_SurveyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyItems_SurveyItemId",
                table: "SurveyAnswers",
                column: "SurveyItemId",
                principalTable: "SurveyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyItemOptions_SurveyItems_SurveyItemId",
                table: "SurveyItemOptions",
                column: "SurveyItemId",
                principalTable: "SurveyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
