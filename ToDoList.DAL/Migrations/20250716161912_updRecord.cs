
#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Record",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Record");
        }
    }
}
