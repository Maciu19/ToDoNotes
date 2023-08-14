using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoNotes.Migrations
{
    /// <inheritdoc />
    public partial class Fifth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Workspace",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 14, 14, 36, 51, 384, DateTimeKind.Local).AddTicks(6779));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Workspace");
        }
    }
}
