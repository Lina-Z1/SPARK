using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string? SkillName { get; set; }
        public string? SkillType { get; set; }


        [ForeignKey("Resume")]
        public int ResumeId { get; set; } 
        public Resume Resume { get; set; }
    }
}
