using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameReviews.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContactFormReplyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedAt",
                table: "ContactForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepliedByuserId",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplyMessage",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedByUserId",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepliedAt",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "RepliedByuserId",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "ReplyMessage",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "SubmittedByUserId",
                table: "ContactForms");
        }
    }
}
