using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class LanguageDTO
    {
        public int LanguageId { get; set; }
       
        public string? LanguageName { get; set; }
        
        public string? LanguageLevel { get; set; }

        public int ResumeId { get; set; }
    }
}
