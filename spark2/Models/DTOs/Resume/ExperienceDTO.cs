using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class ExperienceDTO
    {
        public int ExperienceId { get; set; }      
        public string? CompanyName { get; set; }       
        public string? Position { get; set; }      
        public string? StartDate { get; set; }
                
        public string? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? Duties { get; set; }

        public int ResumeId { get; set; }
        
    }
}
