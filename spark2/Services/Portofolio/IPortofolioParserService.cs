using spark2.Models;
using spark2.Models.Entities;


namespace spark2.Services
{
    public interface IPortofolioParserService
    {
        Task<Portofolio> ParsePortofolioAsync(string rawText);
        Task<Portofolio> ParseAndSaveAsync(string rawText, string personId);
    }
}
