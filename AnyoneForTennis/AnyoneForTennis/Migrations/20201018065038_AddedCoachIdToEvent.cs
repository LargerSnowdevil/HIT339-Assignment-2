using Microsoft.EntityFrameworkCore.Migrations;

namespace AnyoneForTennis.Migrations
{
    public partial class AddedCoachIdToEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Coaches_RunningCoachCoachId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_RunningCoachCoachId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RunningCoachCoachId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CoachId",
                table: "Events",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Coaches_CoachId",
                table: "Events",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "CoachId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Coaches_CoachId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CoachId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "RunningCoachCoachId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_RunningCoachCoachId",
                table: "Events",
                column: "RunningCoachCoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Coaches_RunningCoachCoachId",
                table: "Events",
                column: "RunningCoachCoachId",
                principalTable: "Coaches",
                principalColumn: "CoachId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
