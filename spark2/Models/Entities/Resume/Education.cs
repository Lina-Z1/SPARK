using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Education
    {
        public int EducationId { get; set; }
        public string? UniversityName { get; set; }
        public string? DegreeType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Major { get; set; }

        public string? GPA { get; set; }


        [ForeignKey("Resume")]
        public int ResumeId { get; set; }  
        public Resume Resume { get; set; }
    }
}
