using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineAppointmentSystem.DataAccess.Migrations
{
    public partial class AddNotesToWorkingHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WorkingHours",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WorkingHours");
        }
    }
} 