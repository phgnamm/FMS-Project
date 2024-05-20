using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "Freelancer",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Freelancer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Freelancer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Freelancer");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Freelancer");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "Freelancer",
                newName: "Dob");
        }
    }
}
