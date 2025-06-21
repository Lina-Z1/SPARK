using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public string? CompanyName { get; set; }
        public string? Position { get; set; }
        public string?  StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? Duties { get; set; }


        [ForeignKey("Resume")]
        public int ResumeId { get; set; }  
        public Resume Resume { get; set; }
    }
}
