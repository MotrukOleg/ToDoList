using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModelRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Record");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Record",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Record");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Record",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
