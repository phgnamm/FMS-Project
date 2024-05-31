using Repositories.Interfaces;
using Repositories.Common;
using API.Middlewares;
using System.Diagnostics;
using Repositories.Entities;
using Repositories;
using Services.Interfaces;
using Services.Services;
using Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Services.Common;
using API.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace API
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

            //Skill
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<ISkillRepository, SkillRepository>();



            return services;
		}
	}
}
