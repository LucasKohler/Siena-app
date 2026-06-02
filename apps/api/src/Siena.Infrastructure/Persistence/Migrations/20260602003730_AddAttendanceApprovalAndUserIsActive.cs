using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Siena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceApprovalAndUserIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "attendances",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "Pendente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "attendances");
        }
    }
}
