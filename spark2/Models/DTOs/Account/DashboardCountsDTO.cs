namespace spark2.Models.DTOs.Account
{
    public class DashboardCountsDTO
    {
        public int UserCount { get; set; }
        public int AdminCount { get; set; }
        public int PortfolioCount { get; set; }
        public int ResumeCount { get; set; }
        public int PublishedResumeCount { get; set; }
        public int PublishedPortfolioCount { get; set; }
    }
}
