using ChillDe.FMS.Repositories.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChillDe.FMS.Repositories.Common
{
    /// <summary>
    /// This class is used to insert initial data
    /// </summary>
    public class InitialSeeding
    {
        private static readonly string[] roles =
        {
            Enums.Role.Administrator.ToString(),
            Enums.Role.Staff.ToString()
        };

        private static readonly Dictionary<string, string[]> skills = new Dictionary<string, string[]>
        {
            {
                "Graphic & Design",
                new[]
                {
                    "Brand Identity", "Web & App Design", "Art & Illustration", "Architecture & Building Design",
                    "Product & Gaming", "Visual Design", "Print Design", "Packaging & Covers", "3D Design",
                    "Marketing Design", "Fashion & Merchandise"
                }
            },
            {
                "Programming & Tech",
                new[]
                {
                    "Websites", "Application Development", "Software Development", "Mobile Apps", "Website Platforms",
                    "Support & Cybersecurity", "Blockchain & Cryptocurrency"
                }
            },
            {
                "Digital Marketing",
                new[]
                {
                    "Search", "Social", "Methods & Techniques", "Analytics & Strategy", "Industry & Purpose-Specific"
                }
            },
            {
                "Video & Animation",
                new[]
                {
                    "Editing & Post-Production", "Animation", "Motion Graphics", "Social & Marketing Videos",
                    "Explainer Videos", "Product Videos", "Filmed Video Production"
                }
            },
            {
                "Writing & Translation",
                new[]
                {
                    "Content Writing", "Editing & Critique", "Book & eBook Publishing", "Translation & Transcription",
                    "Business & Marketing Copy", "Career Writing"
                }
            },
            {
                "Music & Audio",
                new[]
                {
                    "Music Production & Writing", "Audio Engineering & Post Production", "Voice Over & Streaming",
                    "Lessons & Transcription", "Sound Design"
                }
            },
            {
                "Business",
                new[]
                {
                    "Business Formation & Growth", "General & Administrative", "Legal Services", "Accounting & Finance",
                    "Sales & Customer Care"
                }
            }
        };

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            if (roleManager != null)
            {
                foreach (string role in roles)
                {
                    Role? existedRole = await roleManager.FindByNameAsync(role);
                    if (existedRole == null)
                    {
                        await roleManager.CreateAsync(new Role { Name = role });
                    }
                }
            }

            if (context != null)
            {
                await context.Database.MigrateAsync();

                foreach (var category in skills.Keys)
                {
                    if (!context.ProjectCategory.Any(x => x.Name == category))
                    {
                        context.ProjectCategory.Add(new ProjectCategory() { Name = category });
                    }

                    foreach (var subSkill in skills[category])
                    {
                        if (!context.Skill.Any(x => x.Name == subSkill && x.Type == category))
                        {
                            context.Skill.Add(new Skill() { Name = subSkill, Type = category });
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}