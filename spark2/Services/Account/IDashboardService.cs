using spark2.Models.DTOs.Account;

namespace spark2.Services.Account
{
    public interface IDashboardService
    {
        Task<DashboardCountsDTO> GetDashboardCountsAsync();
    }
}
