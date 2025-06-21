using Microsoft.AspNetCore.Identity;
using spark2.Models.DTOs.Account;
using spark2.Models.Entities;
using spark2.Repos.ResumeRepositary;
using spark2.Repos;
using Microsoft.EntityFrameworkCore;

namespace spark2.Services.Account
{
    public class DashboardService : IDashboardService
    {

        private readonly UserManager<Person> _userManager;
        private readonly IPortofolioRepo _portfolioRepo;
        private readonly IResumeRepo _resumeRepo;

        public DashboardService(
            UserManager<Person> userManager,
            IPortofolioRepo portfolioRepo,
            IResumeRepo resumeRepo)
        {
            _userManager = userManager;
            _portfolioRepo = portfolioRepo;
            _resumeRepo = resumeRepo;
        }

        public async Task<DashboardCountsDTO>  GetDashboardCountsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var portfolios = await _portfolioRepo.GetAllPortofoliosAsync();
            var resumes = await _resumeRepo.GetAllResumesAsync();

            int adminCount = 0;
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                    adminCount++;
            }

            return new DashboardCountsDTO
            {
                UserCount = users.Count,
                AdminCount = adminCount,
                PortfolioCount = portfolios.Count,
                PublishedPortfolioCount = portfolios.Count(r => r.IsPublished),
                ResumeCount = resumes.Count,
                PublishedResumeCount = resumes.Count(r => r.IsPublished)
            };
        }

    }
}
 


