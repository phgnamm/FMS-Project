using ChillDe.FMS.Repositories.Interfaces;
using Quartz;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Common
{
    public class WarningEmailJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public WarningEmailJob(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var projectApplyId = context.MergedJobDataMap.GetGuid("projectApplyId");

            var projectApply = await _unitOfWork.ProjectApplyRepository.GetAsync(projectApplyId, "Freelancer,Project");
            if (projectApply != null && projectApply.Freelancer != null && projectApply.Project != null)
            {
                string toEmail = projectApply.Freelancer.Email;
                string subject = "Project Deadline Reminder";
                string body = $"Dear {projectApply.Freelancer.FirstName},\n\nThis is a reminder that the deadline for the project '{projectApply.Project.Name}' is tomorrow.\n\nBest regards,\nFMS Managerment";

                await _emailService.SendEmailAsync(toEmail, subject, body, false);

                projectApply.Freelancer.Warning += 1;

                if (projectApply.Freelancer.Warning > 3)
                {
                    projectApply.Freelancer.IsDeleted = true;
                    string lockSubject = "Account Locked";
                    string lockBody = $"Dear {projectApply.Freelancer.FirstName},\n\nYour account has been locked due to exceeding the maximum number of warnings.\n\nBest regards,\nYour Company";
                    await _emailService.SendEmailAsync(toEmail, lockSubject, lockBody, false);
                }

                _unitOfWork.FreelancerRepository.Update(projectApply.Freelancer);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
