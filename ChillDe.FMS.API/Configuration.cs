using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Common;
using System.Diagnostics;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories;
using Services.Interfaces;
using ChillDe.FMS.Services;
using ChillDe.FMS.Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Services.Common;
using ChillDe.FMS.API.Middlewares;
using ChillDe.FMS.API.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Services.Services;
using Repositories.Interfaces;
using Repositories.Repositories;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Services;
using Quartz;
using ChillDe.FMS.Services.Common;
using Quartz.Impl;

namespace ChillDe.FMS.API
{
	public static class Configuration
	{
		public static IServiceCollection AddAPIConfiguration(this IServiceCollection services)
		{
			// Identity
			services
				.AddIdentity<Account, Role>(options =>
				{
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequiredLength = 8;
				})
				.AddRoles<Role>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();
			services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromMinutes(15);
			});
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("credentials.json"),
            });

            // Đăng ký Quartz.NET
            services.AddQuartz(configure =>
            {
                configure.UseMicrosoftDependencyInjectionJobFactory();
				var jobKey = new JobKey("WarningEmailJob");
				configure.AddJob<WarningEmailJob>(opts => opts.WithIdentity(jobKey));

				// Tạo trigger cho công việc gửi email cảnh báo
				configure.AddTrigger(opts => opts
					.ForJob(jobKey)
					.WithIdentity("WarningEmailJob-trigger")
					.WithCronSchedule("0 0 8 * * ?")); 
			});

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            // Đăng ký ISchedulerFactory và IScheduler
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton(provider =>
            {
                var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
                var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
                scheduler.Start().GetAwaiter().GetResult();
                return scheduler;
            });

            // Middlewares
            services.AddSingleton<GlobalExceptionMiddleware>();
			services.AddSingleton<PerformanceMiddleware>();
			services.AddSingleton<Stopwatch>();

			// Common
			services.AddHttpContextAccessor();
			services.AddAutoMapper(typeof(MapperProfile).Assembly);
			services.AddScoped<IClaimsService, ClaimsService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IEmailService, EmailService>();

			// Dependency Injection
			// Account
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IAccountRepository, AccountRepository>();
			
			
			// Freelancer
			services.AddScoped<IFreelancerService, FreelancerService>();
			services.AddScoped<IFreelancerRepository, FreelancerRepository>();
			
			// Project
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IProjectService, ProjectService>();

			//ProjectDeliverable
			services.AddScoped<IProjectDeliverableRepository, ProjectDeliverableRepository>();
			services.AddScoped<IProjectDeliverableService, ProjectDeliverableService>();

            //DeliverableType
            services.AddScoped<IDeliverableTypeRepository, DeliverableTypeRepository>();
            services.AddScoped<IDeliverableTypeService, DeliverableTypeService>();

            //ProjectCategory
            services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();
			services.AddScoped<IProjectCategoryService, ProjectCategoryService>();

            //ProjectApply
            services.AddScoped<IProjectApplyRepository, ProjectApplyRepository>();
            services.AddScoped<IProjectApplyService, ProjectApplyService>();

            //DeliverableProduct
            services.AddScoped<IDeliverableProductRepository, DeliverableProductRepository>();
            services.AddScoped<IDeliverableProductService, DeliverableProductService>();

            // Skill
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<ISkillRepository, SkillRepository>();

			// FreelancerSkill
			services.AddScoped<IFreelancerSkillRepository, FreelancerSkillRepository>();

			// Transaction
			services.AddScoped<ITransactionService, TransactionService>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
		}
	}
}
