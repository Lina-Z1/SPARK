using spark2.Models;
using spark2.Models.Entities;

namespace spark2.Services
{
    public interface IResumeParserService
    {
        Task<Resume> ParseResumeAsync(string rawText);
        Task<Resume> ParseAndSaveAsync(string rawText, string personId);
    }
}
