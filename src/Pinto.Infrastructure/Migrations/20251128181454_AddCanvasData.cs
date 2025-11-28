using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCanvasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanvasData",
                table: "Boards",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanvasData",
                table: "Boards");
        }
    }
}
