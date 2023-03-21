using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingAppleConsumer.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionToShowHideResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowResultsOnCompletion",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowResultsOnCompletion",
                table: "Surveys");
        }
    }
}
