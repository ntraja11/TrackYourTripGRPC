using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackYourTripGRPCApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyFromMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Trips_TripId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_TripId",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Members_TripId",
                table: "Members",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Trips_TripId",
                table: "Members",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
