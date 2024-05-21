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
				entity.HasIndex(x => x.Code).IsUnique();
				
				entity.Property(x => x.Name).HasMaxLength(256);
				entity.Property(x => x.Status).HasMaxLength(50);
			});
			
			modelBuilder.Entity<Freelancer>(entity =>
			{
				entity.HasIndex(x => x.Code).IsUnique();
                
				entity.Property(x => x.FirstName).HasMaxLength(50);
				entity.Property(x => x.LastName).HasMaxLength(50);
				entity.Property(x => x.PhoneNumber).HasMaxLength(15);
				entity.Property(x => x.Status).HasMaxLength(50);
				entity.Property(x => x.Email).HasMaxLength(256);
			});
			
			modelBuilder.Entity<ProjectCategory>(entity =>
			{
				entity.Property(x => x.Name).HasMaxLength(256);
			});
			
			modelBuilder.Entity<ProjectDeliverable>(entity =>
			{
				entity.Property(x => x.Name).HasMaxLength(256);
				entity.Property(x => x.Status).HasMaxLength(50);
			});
			
			modelBuilder.Entity<DeliverableProduct>(entity =>
			{
				entity.Property(x => x.Name).HasMaxLength(256);
				entity.Property(x => x.Status).HasMaxLength(50);
			});
			
			modelBuilder.Entity<DeliverableType>(entity =>
			{
				entity.Property(x => x.Name).HasMaxLength(256);
			});
			
			modelBuilder.Entity<ProjectApply>(entity =>
			{
				entity.Property(x => x.Status).HasMaxLength(50);
			});
			
			modelBuilder.Entity<Transaction>(entity =>
			{
				entity.HasIndex(x => x.Code).IsUnique();
			});
			
			modelBuilder.Entity<Skill>(entity =>
			{
				entity.Property(x => x.Name).HasMaxLength(256);
				entity.Property(x => x.Type).HasMaxLength(50);
			});
		}
	}
}
