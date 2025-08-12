using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameReviews.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminResponseAndResponseDateToGameRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminResponse",
                table: "GameRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "GameRequests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminResponse",
                table: "GameRequests");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "GameRequests");
        }
    }
}
