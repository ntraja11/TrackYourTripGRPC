using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackYourTripGRPCApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupIdToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Trips");
        }
    }
}
