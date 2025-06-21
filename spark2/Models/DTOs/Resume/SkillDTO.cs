using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class SkillDTO
    {
        public int SkillId { get; set; }      
        public string? SkillName { get; set; }
        public string? SkillType { get; set; }

        public int ResumeId { get; set; }
         
    }
}
