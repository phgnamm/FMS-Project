using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class AppDbContext : IdentityDbContext<Account, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Freelancer> Freelancer { get; set; }
        public DbSet<Skill> Skill { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();

                entity.Property(x => x.FirstName).HasMaxLength(50);
                entity.Property(x => x.LastName).HasMaxLength(50);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.VerificationCode).HasMaxLength(6);

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(x => x.Description).HasMaxLength(256);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");
                entity.HasKey(x => x.Id).HasName("Project_pk");
                entity.HasIndex(x => x.Code).IsUnique();
                entity.Property(x => x.Name).HasMaxLength(256);
                entity.Property(x => x.Status).HasMaxLength(50);

                entity.HasOne(x => x.Account).WithMany(x => x.Projects)
               .HasForeignKey(x => x.AccountId)
               .HasConstraintName("Project_Account_ProjectId_fk");

                entity.HasOne(x => x.ProjectCategory).WithMany(x => x.Projects)
               .HasForeignKey(x => x.CategoryId)
               .HasConstraintName("Project_ProjectCategory_ProjectId_fk");
            });

            modelBuilder.Entity<Freelancer>(entity =>
            {
                entity.ToTable("Freelancer");
                entity.HasKey(x => x.Id).HasName("Freelancer_pk");
                entity.HasIndex(x => x.Code).IsUnique();

                entity.Property(x => x.FirstName).HasMaxLength(50);
                entity.Property(x => x.LastName).HasMaxLength(50);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.Status).HasMaxLength(50);
                entity.Property(x => x.Email).HasMaxLength(256);
            });

            modelBuilder.Entity<ProjectCategory>(entity =>
            {
                entity.ToTable("ProjectCategory");
                entity.HasKey(x => x.Id).HasName("ProjectCategory_pk");
                entity.Property(x => x.Name).HasMaxLength(256);

            });

            modelBuilder.Entity<ProjectDeliverable>(entity =>
            {
                entity.ToTable("ProjectDeliverable");
                entity.HasKey(x => x.Id).HasName("ProjectDeliverable_pk");
                entity.Property(x => x.Name).HasMaxLength(256);
                entity.Property(x => x.Status).HasMaxLength(50);

                entity.HasOne(x => x.Project).WithMany(x => x.ProjectDeliverables)
              .HasForeignKey(x => x.ProjectId)
              .HasConstraintName("ProjectDeliverable_Project_ProjectId_fk");

                entity.HasOne(x => x.DeliverableType).WithMany(x => x.ProjectDeliverable)
               .HasForeignKey(x => x.DeliverableTypeId)
               .HasConstraintName("ProjectDeliverable_DeliverableType_DeliverableTypeId_fk");
            });

            modelBuilder.Entity<DeliverableProduct>(entity =>
            {
                entity.ToTable("DeliverableProduct");
                entity.HasKey(x => x.Id).HasName("DeliverableProduct_pk");
                entity.Property(x => x.Name).HasMaxLength(256);
                entity.Property(x => x.Status).HasMaxLength(50);

                entity.HasOne(x => x.ProjectApply).WithMany(x => x.DeliverableProducts)
               .HasForeignKey(x => x.ProjectApplyId)
               .HasConstraintName("DeliverableProduct_ProjectApply_ProjectApplyId_fk");

                entity.HasOne(x => x.ProjectDeliverable).WithMany(x => x.DeliverableProducts)
               .HasForeignKey(x => x.ProjectDeliverableId)
               .HasConstraintName("DeliverableProduct_ProjectDeliverable_ProjectDeliverableId_fk");
            });

            modelBuilder.Entity<DeliverableType>(entity =>
            {
                entity.ToTable("DeliverableType");
                entity.HasKey(x => x.Id).HasName("DeliverableType_pk");
                entity.Property(x => x.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<ProjectApply>(entity =>
            {
                entity.ToTable("ProjectApply");
                entity.HasKey(x => x.Id).HasName("ProjectApply_pk");
                entity.Property(x => x.Status).HasMaxLength(50);

                entity.HasOne(x => x.Project).WithMany(x => x.ProjectApplies)
               .HasForeignKey(x => x.ProjectId)
               .HasConstraintName("ProjectApply_Project_ProjectId_fk");

                entity.HasOne(x => x.Freelancer).WithMany(x => x.ProjectApplies)
               .HasForeignKey(x => x.FreelancerId)
               .HasConstraintName("ProjectApply_Freelancer_FreelancerId_fk");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(x => x.Id).HasName("Transaction_pk");
                entity.HasIndex(x => x.Code).IsUnique();

                entity.HasOne(x => x.Project).WithMany(x => x.Transactions)
               .HasForeignKey(x => x.ProjectId)
               .HasConstraintName("Transaction_Project_ProjectId_fk");

                entity.HasOne(x => x.Freelancer).WithMany(x => x.Transactions)
               .HasForeignKey(x => x.FreelancerId)
               .HasConstraintName("Transaction_Freelancer_FreelancerId_fk");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");
                entity.HasKey(x => x.Id).HasName("Skill_pk");
                entity.Property(x => x.Name).HasMaxLength(256);
                entity.Property(x => x.Type).HasMaxLength(50);
            });
            modelBuilder.Entity<FreelancerSkill>(entity =>
            {
                entity.ToTable("FreelancerSkill");
                entity.HasKey(x => x.Id).HasName("FreelancerSkill_pk");

                entity.HasOne(x => x.Freelancer).WithMany(x => x.FreelancerSkills)
               .HasForeignKey(x => x.FreelancerId)
               .HasConstraintName("FreelancerSkill_Freelancer_FreelancerId_fk");

                entity.HasOne(x => x.Skill).WithMany(x => x.FreelancerSkills)
               .HasForeignKey(x => x.SkillId)
               .HasConstraintName("FreelancerSkill_Skill_SkillId_fk");
            });
        }
    }
}
