using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageLevel { get; set; }


        [ForeignKey("Resume")]
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
