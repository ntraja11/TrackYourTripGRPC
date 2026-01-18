using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackYourTripGRPCApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureForeignKeysInExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expenses_MemberId",
                table: "Expenses",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TripId",
                table: "Expenses",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_MemberId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TripId",
                table: "Expenses");
        }
    }
}
