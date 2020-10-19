using Microsoft.EntityFrameworkCore.Migrations;

namespace AnyoneForTennis.Migrations
{
    public partial class AddedEventMembersPersistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMember_Events_EventId",
                table: "EventMember");

            migrationBuilder.DropForeignKey(
                name: "FK_EventMember_Members_MemberId",
                table: "EventMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMember",
                table: "EventMember");

            migrationBuilder.RenameTable(
                name: "EventMember",
                newName: "EventMembers");

            migrationBuilder.RenameIndex(
                name: "IX_EventMember_MemberId",
                table: "EventMembers",
                newName: "IX_EventMembers_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers",
                columns: new[] { "EventId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventMembers_Events_EventId",
                table: "EventMembers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventMembers_Members_MemberId",
                table: "EventMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMembers_Events_EventId",
                table: "EventMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventMembers_Members_MemberId",
                table: "EventMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers");

            migrationBuilder.RenameTable(
                name: "EventMembers",
                newName: "EventMember");

            migrationBuilder.RenameIndex(
                name: "IX_EventMembers_MemberId",
                table: "EventMember",
                newName: "IX_EventMember_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMember",
                table: "EventMember",
                columns: new[] { "EventId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventMember_Events_EventId",
                table: "EventMember",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventMember_Members_MemberId",
                table: "EventMember",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
