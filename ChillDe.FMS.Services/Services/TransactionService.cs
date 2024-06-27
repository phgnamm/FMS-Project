using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChillDe.FMS.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<Pagination<TransactionDetailModel>> GetTransactionByFilter(TransactionFilterModel transactionFilterModel)
        {
            // Lấy danh sách giao dịch từ repository với các bộ lọc được áp dụng
            var transactionList = await _unitOfWork.TransactionRepository.GetAllAsync(
               filter: x =>
                    (x.IsDeleted == transactionFilterModel.IsDeleted) &&
                    (transactionFilterModel.ProjectId == null || x.ProjectId == transactionFilterModel.ProjectId) &&
                    (transactionFilterModel.FreelancerId == null || x.FreelancerId == transactionFilterModel.FreelancerId) &&
                    (transactionFilterModel.Status == null || x.Status == transactionFilterModel.Status) &&
                    (transactionFilterModel.MinPrice == null || x.Price >= transactionFilterModel.MinPrice) &&
                    (transactionFilterModel.MaxPrice == null || x.Price <= transactionFilterModel.MaxPrice) &&
                    (string.IsNullOrEmpty(transactionFilterModel.Search) ||
                        x.Description.Contains(transactionFilterModel.Search) ||
                        x.Code.Contains(transactionFilterModel.Search)),
                orderBy: query =>
                {
                    switch (transactionFilterModel.Order.ToLower())
                    {
                        case "price":
                            return transactionFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.Price)
                                : query.OrderBy(x => x.Price);
                        case "creation-date":
                        default:
                            return transactionFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.CreationDate)
                                : query.OrderBy(x => x.CreationDate);
                    }
                },
                pageIndex: transactionFilterModel.PageIndex,
                pageSize: transactionFilterModel.PageSize,
                includeProperties: "Project,Freelancer"
            );

            // Nếu danh sách giao dịch không rỗng, chuyển đổi sang TransactionDetailModel
            if (transactionList != null)
            {
                var transactionDetailList = transactionList.Data
                    .Select(t => new TransactionDetailModel
                    {
                        Code = t.Code,
                        Description = t.Description,
                        Status = t.Status == true,
                        Price = t.Price,
                        FreelancerFirstName = t.Freelancer?.FirstName ?? string.Empty,
                        FreelancerLastName = t.Freelancer?.LastName ?? string.Empty,
                        ProjectName = t.Project?.Name ?? string.Empty,
                        ProjectId = t.ProjectId,
                        FreelancerId = t.FreelancerId
                    }).ToList();

                // Trả về kết quả phân trang
                return new Pagination<TransactionDetailModel>(transactionDetailList,
                    transactionList.TotalCount, transactionFilterModel.PageIndex,
                    transactionFilterModel.PageSize);
            }
            return null;
        }

        public async Task<ResponseModel> SubmitProject(Guid projectApplyId)
        {
            var response = new ResponseModel();

            try
            {
                // Lấy thông tin ProjectApply từ repository
                var projectApply = await _unitOfWork.ProjectApplyRepository.GetAsync(projectApplyId, "Project,Freelancer");

                if (projectApply == null)
                {
                    response.Message = "ProjectApply not found.";
                    return response;
                }

                // Tạo transaction từ ProjectApply
                var transaction = new Repositories.Entities.Transaction
                {
                    ProjectId = projectApply.ProjectId,
                    FreelancerId = projectApply.FreelancerId,
                    Price = projectApply.Project.Price ?? 0,
                    Description = $"Submit for project {projectApply.Project.Name} by {projectApply.Freelancer.FirstName} ",
                    Status = true,
                    Project = projectApply.Project,
                    Freelancer = projectApply.Freelancer,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = _claimsService.GetCurrentUserId
                    // Các thuộc tính khác nếu có
                };
                // Lưu transaction vào repository
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                int saveChange = await _unitOfWork.SaveChangeAsync();
                if (saveChange > 0)
                {
                    response.Status = true;
                    response.Message = "Project submitted successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

    }
}
