using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class updateappdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableProduct_ProjectApply_ProjectApplyId",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableProduct_ProjectDeliverable_ProjectDeliverableId",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkill_Freelancer_FreelancerId",
                table: "FreelancerSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkill_Skill_SkillId",
                table: "FreelancerSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_AccountId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectCategory_CategoryId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApply_Freelancer_FreelancerId",
                table: "ProjectApply");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApply_Project_ProjectId",
                table: "ProjectApply");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverable_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverable_Project_ProjectId",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Freelancer_FreelancerId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Project_ProjectId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDeliverable",
                table: "ProjectDeliverable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectCategory",
                table: "ProjectCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectApply",
                table: "ProjectApply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerSkill",
                table: "FreelancerSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Freelancer",
                table: "Freelancer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliverableType",
                table: "DeliverableType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliverableProduct",
                table: "DeliverableProduct");

            migrationBuilder.AddPrimaryKey(
                name: "Transaction_pk",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "Skill_pk",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "ProjectDeliverable_pk",
                table: "ProjectDeliverable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "ProjectCategory_pk",
                table: "ProjectCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "ProjectApply_pk",
                table: "ProjectApply",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "Project_pk",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "FreelancerSkill_pk",
                table: "FreelancerSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "Freelancer_pk",
                table: "Freelancer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "DeliverableType_pk",
                table: "DeliverableType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "DeliverableProduct_pk",
                table: "DeliverableProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "DeliverableProduct_ProjectApply_ProjectApplyId_fk",
                table: "DeliverableProduct",
                column: "ProjectApplyId",
                principalTable: "ProjectApply",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "DeliverableProduct_ProjectDeliverable_ProjectDeliverableId_fk",
                table: "DeliverableProduct",
                column: "ProjectDeliverableId",
                principalTable: "ProjectDeliverable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FreelancerSkill_Freelancer_FreelancerId_fk",
                table: "FreelancerSkill",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FreelancerSkill_Skill_SkillId_fk",
                table: "FreelancerSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Project_Account_ProjectId_fk",
                table: "Project",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Project_ProjectCategory_ProjectId_fk",
                table: "Project",
                column: "CategoryId",
                principalTable: "ProjectCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "ProjectApply_Freelancer_FreelancerId_fk",
                table: "ProjectApply",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "ProjectApply_Project_ProjectId_fk",
                table: "ProjectApply",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "ProjectDeliverable_DeliverableType_DeliverableTypeId_fk",
                table: "ProjectDeliverable",
                column: "DeliverableTypeId",
                principalTable: "DeliverableType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "ProjectDeliverable_Project_ProjectId_fk",
                table: "ProjectDeliverable",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Transaction_Freelancer_FreelancerId_fk",
                table: "Transaction",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Transaction_Project_ProjectId_fk",
                table: "Transaction",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DeliverableProduct_ProjectApply_ProjectApplyId_fk",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "DeliverableProduct_ProjectDeliverable_ProjectDeliverableId_fk",
                table: "DeliverableProduct");

            migrationBuilder.DropForeignKey(
                name: "FreelancerSkill_Freelancer_FreelancerId_fk",
                table: "FreelancerSkill");

            migrationBuilder.DropForeignKey(
                name: "FreelancerSkill_Skill_SkillId_fk",
                table: "FreelancerSkill");

            migrationBuilder.DropForeignKey(
                name: "Project_Account_ProjectId_fk",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "Project_ProjectCategory_ProjectId_fk",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "ProjectApply_Freelancer_FreelancerId_fk",
                table: "ProjectApply");

            migrationBuilder.DropForeignKey(
                name: "ProjectApply_Project_ProjectId_fk",
                table: "ProjectApply");

            migrationBuilder.DropForeignKey(
                name: "ProjectDeliverable_DeliverableType_DeliverableTypeId_fk",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "ProjectDeliverable_Project_ProjectId_fk",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "Transaction_Freelancer_FreelancerId_fk",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "Transaction_Project_ProjectId_fk",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "Transaction_pk",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "Skill_pk",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "ProjectDeliverable_pk",
                table: "ProjectDeliverable");

            migrationBuilder.DropPrimaryKey(
                name: "ProjectCategory_pk",
                table: "ProjectCategory");

            migrationBuilder.DropPrimaryKey(
                name: "ProjectApply_pk",
                table: "ProjectApply");

            migrationBuilder.DropPrimaryKey(
                name: "Project_pk",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "FreelancerSkill_pk",
                table: "FreelancerSkill");

            migrationBuilder.DropPrimaryKey(
                name: "Freelancer_pk",
                table: "Freelancer");

            migrationBuilder.DropPrimaryKey(
                name: "DeliverableType_pk",
                table: "DeliverableType");

            migrationBuilder.DropPrimaryKey(
                name: "DeliverableProduct_pk",
                table: "DeliverableProduct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDeliverable",
                table: "ProjectDeliverable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectCategory",
                table: "ProjectCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectApply",
                table: "ProjectApply",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerSkill",
                table: "FreelancerSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Freelancer",
                table: "Freelancer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliverableType",
                table: "DeliverableType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliverableProduct",
                table: "DeliverableProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableProduct_ProjectApply_ProjectApplyId",
                table: "DeliverableProduct",
                column: "ProjectApplyId",
                principalTable: "ProjectApply",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableProduct_ProjectDeliverable_ProjectDeliverableId",
                table: "DeliverableProduct",
                column: "ProjectDeliverableId",
                principalTable: "ProjectDeliverable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkill_Freelancer_FreelancerId",
                table: "FreelancerSkill",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkill_Skill_SkillId",
                table: "FreelancerSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_AccountId",
                table: "Project",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ProjectCategory_CategoryId",
                table: "Project",
                column: "CategoryId",
                principalTable: "ProjectCategory",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverable_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverable",
                column: "DeliverableTypeId",
                principalTable: "DeliverableType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverable_Project_ProjectId",
                table: "ProjectDeliverable",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Freelancer_FreelancerId",
                table: "Transaction",
                column: "FreelancerId",
                principalTable: "Freelancer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Project_ProjectId",
                table: "Transaction",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }
    }
}
