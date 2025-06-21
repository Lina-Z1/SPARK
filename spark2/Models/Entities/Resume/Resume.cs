using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Resume
    {
        public int ResumeId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Summary { get; set; }
        public string? Address { get; set; } 
        public string? Title { get; set; }
        public string? Link1 { get; set; }  
        public string? Link2 { get; set; }  
        public string? Link3 { get; set; } 

        public bool IsPublished { get; set; } = false;


        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Language> Languages { get; set; } = new List<Language>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();


        [ForeignKey("Person")]
        public string PersonId { get; set; }
        public Person Person { get; set; }

    }
}
