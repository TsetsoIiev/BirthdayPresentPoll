using Microsoft.EntityFrameworkCore.Migrations;

namespace BirthdayPresentPoll.Web.Data.Migrations
{
    public partial class UpdatePollVoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVote_Votes_VoteId",
                table: "PollVote");

            migrationBuilder.DropColumn(
                name: "VodeId",
                table: "PollVote");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "PollVote",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PollVote_Votes_VoteId",
                table: "PollVote",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVote_Votes_VoteId",
                table: "PollVote");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "PollVote",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VodeId",
                table: "PollVote",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PollVote_Votes_VoteId",
                table: "PollVote",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
