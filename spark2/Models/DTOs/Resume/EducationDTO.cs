using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class EducationDTO
    {
        public int EducationId { get; set; }        
        public string? UniversityName { get; set; }
        public string? DegreeType { get; set; } = "";
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }        
        public string? Major { get; set; } = "";


        [Range(0, 4.0, ErrorMessage = "GPA must be between 0 and 4.0")]
        public string? GPA { get; set; }

        public int ResumeId { get; set; }

      

    }

}
