namespace ChillDe.FMS.Repositories.Enums;

public enum ProjectStatus
{
    Pending, // Mới được tạo, đang tuyển Freelancer
    Processing, // Đang được thực hiện
    Checking, // Đang chờ duyệt sau khi Freelancer hoàn thành sản phẩm
    Closed, // Dự án bị đóng
    Done // Dự án hoàn thành
}