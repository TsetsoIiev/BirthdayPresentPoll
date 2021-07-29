using Microsoft.EntityFrameworkCore.Migrations;

namespace BirthdayPresentPoll.Web.Data.Migrations
{
    public partial class UpdatePollTableAddIsActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Polls",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Polls");
        }
    }
}
