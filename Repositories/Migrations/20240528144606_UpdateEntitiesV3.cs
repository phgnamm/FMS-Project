using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableProduct_ProjectApply_ProjectApplyId",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApply_Freelancer_FreelancerId",
                table: "ProjectApply");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApply_Project_ProjectId",
                table: "ProjectApply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectApply",
                table: "ProjectApply");

            migrationBuilder.RenameTable(
                name: "ProjectApply",
                newName: "ProjectApplie");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApply_ProjectId",
                table: "ProjectApplie",
                newName: "IX_ProjectApplie_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApply_FreelancerId",
                table: "ProjectApplie",
                newName: "IX_ProjectApplie_FreelancerId");

            migrationBuilder.AddColumn<int>(
                name: "Warning",
                table: "Freelancer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ProjectApplie",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ProjectApplie",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectApplie",
                table: "ProjectApplie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableProduct_ProjectApplie_ProjectApplyId",
                table: "DeliverableProduct",
                column: "ProjectApplyId",
                principalTable: "ProjectApplie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplie_Freelancer_FreelancerId",
                table: "ProjectApplie",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplie_Project_ProjectId",
                table: "ProjectApplie",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableProduct_ProjectApplie_ProjectApplyId",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplie_Freelancer_FreelancerId",
                table: "ProjectApplie");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplie_Project_ProjectId",
                table: "ProjectApplie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectApplie",
                table: "ProjectApplie");

            migrationBuilder.DropColumn(
                name: "Warning",
                table: "Freelancer");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ProjectApplie");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ProjectApplie");

            migrationBuilder.RenameTable(
                name: "ProjectApplie",
                newName: "ProjectApply");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplie_ProjectId",
                table: "ProjectApply",
                newName: "IX_ProjectApply_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplie_FreelancerId",
                table: "ProjectApply",
                newName: "IX_ProjectApply_FreelancerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectApply",
                table: "ProjectApply",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableProduct_ProjectApply_ProjectApplyId",
                table: "DeliverableProduct",
                column: "ProjectApplyId",
                principalTable: "ProjectApply",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApply_Freelancer_FreelancerId",
                table: "ProjectApply",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApply_Project_ProjectId",
                table: "ProjectApply",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }
    }
}
